using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TradeCube_Services.Constants;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Exceptions;
using TradeCube_Services.Helpers;
using TradeCube_Services.Messages;
using TradeCube_Services.Parameters;
using TradeCube_Services.ThirdParty.Enegen;

namespace TradeCube_Services.Services
{
    public class EnegenAtomService : IEnegenAtomService
    {
        private readonly ITradeService tradeService;
        private readonly ITradeProfileService tradeProfileService;
        private readonly ILogger<EnegenAtomService> logger;

        public EnegenAtomService(ITradeService tradeService, ITradeProfileService tradeProfileService, ILogger<EnegenAtomService> logger)
        {
            this.tradeService = tradeService;
            this.tradeProfileService = tradeProfileService;
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<WebServiceResponse>> Trade(EnegenAtomTradeParameters enegenAtomTradeParameters)
        {
            try
            {
                var apiJwtToken = enegenAtomTradeParameters.ApiJwtToken;
                var request = new TradeRequest { TradeReferences = enegenAtomTradeParameters.TradeReferences };
                var trades = await tradeService.Trades(apiJwtToken, request);

                if (trades.Status == ApiConstants.SuccessResult)
                {
                    await ProcessTrades(trades.Data, apiJwtToken);

                    return new ApiResponseWrapper<WebServiceResponse>
                    {
                        Status = ApiConstants.SuccessResult,
                        Data = new WebServiceResponse()
                    };
                }

                logger.LogError("Error calling Trade API", trades.Message);
                return new ApiResponseWrapper<WebServiceResponse> { Status = ApiConstants.FailedResult, Message = trades.Message };
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<WebServiceResponse> { Status = ApiConstants.FailedResult, Message = e.Message };
            }
        }

        private async Task ProcessTrades(IEnumerable<TradeDataObject> trades, string apiJwtToken)
        {
            var tradeList = trades.ToList();
            var apiResponseWrappers = await GetProfiles(tradeList, apiJwtToken).ToListAsync();

            var profileResponses = apiResponseWrappers
                .Where(d => d.Data != null && d.Data.Any())
                .SelectMany(p => p.Data);

            var inboundTrades = CreateEnegen(profileResponses, tradeList);

            logger.LogDebug(JsonSerializer.Serialize(inboundTrades));
        }

        private async IAsyncEnumerable<ApiResponseWrapper<IEnumerable<TradeProfileResponse>>> GetProfiles(IEnumerable<TradeDataObject> trades, string apiJwtToken)
        {
            foreach (var trade in trades)
            {
                var tradeProfileRequest = new TradeProfileRequest
                {
                    TradeReference = trade.TradeReference,
                    TradeLeg = trade.TradeLeg,
                    ProfileFormat = ProfileRequestFormat.Full,
                    Volume = true,
                    Price = true
                };

                yield return await tradeProfileService.TradeProfiles(apiJwtToken, tradeProfileRequest);
            }
        }

        private static IEnumerable<EnegenInboundTrade> CreateEnegen(IEnumerable<TradeProfileResponse> tradeProfileResponses, IEnumerable<TradeDataObject> trades)
        {
            return new List<EnegenInboundTrade>
            {
                new EnegenInboundTrade
                {
                    ScheduleDates = CreateScheduleDates(tradeProfileResponses, trades)
                }
            };
        }

        private static IEnumerable<EnegenScheduleDate> CreateScheduleDates(IEnumerable<TradeProfileResponse> tradeProfileResponses, IEnumerable<TradeDataObject> trades)
        {
            static DateTime LocalDateTime(DateTime dt, TradeProfileResponse tpr, ILookup<(string TradeReference, int TradeLeg), TradeDataObject> lookup)
            {
                var trade = lookup[(tpr.TradeReference, tpr.TradeLeg)].SingleOrDefault();
                if (trade == null)
                {
                    throw new TradeProfileException($"Trade '{tpr.TradeReference},{tpr.TradeLeg}' not found'");
                }

                var timezone = trade.Product?.Commodity?.Timezone;
                if (timezone == null)
                {
                    throw new TradeProfileException("Trade timezone not set");
                }

                return DateTimeHelper.GetLocalDateTime(dt, timezone);
            }

            static IEnumerable<(string TradeReference, int TradeLeg, DateTime Date, decimal Volume, decimal Price)> Combine(IEnumerable<TradeProfileResponse> profileResponses,
                ILookup<(string TradeReference, int TradeLeg), TradeDataObject> lookup)
            {
                foreach (var profileResponse in profileResponses)
                {
                    var volumeProfile = profileResponse.VolumeProfile.ToList();
                    var priceProfile = profileResponse.PriceProfile.ToList();

                    if (volumeProfile.Count != priceProfile.Count)
                    {
                        throw new TradeProfileException("Volume/Price number mismatch");
                    }

                    for (var vp = 0; vp < volumeProfile.Count; vp++)
                    {
                        var volume = volumeProfile[vp];
                        var price = priceProfile[vp];

                        yield return volume.UtcStartDateTime == price.UtcStartDateTime
                            ? (profileResponse.TradeReference, profileResponse.TradeLeg, LocalDateTime(volume.UtcStartDateTime, profileResponse, lookup), volume.Value, price.Value)
                            : throw new TradeProfileException("Volume/Price date/time mismatch");
                    }
                }
            }

            static IEnumerable<EnegenScheduleTrade> TradesOnDate(IEnumerable<(string TradeReference, int TradeLeg, DateTime Date, decimal Volume, decimal Price)> data,
                ILookup<(string TradeReference, int TradeLeg), TradeDataObject> lookup)
            {
                var byTrade = data
                    .GroupBy(p => (p.TradeReference, p.TradeLeg), p => p)
                    .Select(g => new { Trade = g.Key, Data = g.ToList() });

                foreach (var trade in byTrade)
                {
                    yield return new EnegenScheduleTrade
                    {
                        TradeId = HashHelper.HashStringToInteger($"{trade.Trade.TradeReference}{trade.Trade.TradeLeg}"),
                        Version = 1,
                        Counterparty = lookup[(trade.Trade.TradeReference, trade.Trade.TradeLeg)].SingleOrDefault()?.Counterparty?.Party,
                        ScheduleTradeDetails = trade.Data.Select((t, i) => new EnegenScheduleTradeDetail
                        {
                            SettlementPeriod = i + 1,
                            Volume = t.Volume,
                            Price = t.Price
                        })
                    };
                }
            }

            var tradeLookup = trades.ToLookup(t => (t.TradeReference, t.TradeLeg), p => p);
            var combined = Combine(tradeProfileResponses, tradeLookup);

            var groupedByDate = combined
                .GroupBy(p => p.Date.Date, p => p)
                .Select(g => new { Date = g.Key, Data = g.ToList() })
                .OrderBy(d => d.Date);

            foreach (var byDate in groupedByDate)
            {
                yield return new EnegenScheduleDate
                {
                    Date = byDate.Date,
                    ScheduleTrades = TradesOnDate(byDate.Data, tradeLookup)
                };
            }
        }

        private void InsertTrade(string environment, IEnumerable<EnegenInboundTrade> inboundTrades)
        {
        }
    }
}