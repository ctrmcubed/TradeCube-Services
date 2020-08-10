using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeCube_Services.Constants;
using TradeCube_Services.DataObjects;
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
            var apiResponseWrappers = await GetProfiles(trades, apiJwtToken).ToListAsync();
            var profileResponses = apiResponseWrappers.SelectMany(p => p.Data);

            var t = CreateEnegen(profileResponses, trades);
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
            var tradeLookup = trades.ToLookup(t => (t.TradeReference, t.TradeLeg), p => p);
            var profilesWithTrades = tradeProfileResponses.Select(t => (t, tradeLookup[(t.TradeReference, t.TradeLeg)]));

            foreach (var (tradeProfileResponse, tradeDataObjects) in profilesWithTrades)
            {
                var tradeDataObject = tradeDataObjects.SingleOrDefault();
                if (tradeDataObject == null)
                {
                    throw new Exception();
                }

                var volumesByDate = tradeProfileResponse.VolumeProfile
                    .GroupBy(p => p.UtcStartDateTime, p => p.Value)
                    .Select(g => new { Date = g.Key, Volumes = g.ToList() })
                    .OrderBy(d => d.Date);

                var pricesByDate = tradeProfileResponse.PriceProfile
                    .GroupBy(p => p.UtcStartDateTime, p => p.Value)
                    .Select(g => new { Date = g.Key, Prices = g.ToList() })
                    .OrderBy(d => d.Date);

                var combineByDate = volumesByDate.EquiZip(pricesByDate, (v, p) => (v.Date, v.Volumes, p.Prices));

                foreach (var (date, volumes, prices) in combineByDate)
                {
                    var combineValues = volumes.EquiZip(prices, (v, p) => (v, p));

                    yield return new EnegenScheduleDate
                    {
                        Date = date,
                        ScheduleTrades = combineValues.Select((tuple, index) => new EnegenScheduleTrade
                        {
                            // TODO
                            TradeId = HashHelper.HashStringToInteger($"{tradeDataObject.TradeReference}{tradeDataObject.TradeLeg}"),
                            Version = 1,
                            //Counterparty = trade.Counterparty
                            ScheduleTradeDetails = new List<EnegenScheduleTradeDetail>
                            {
                                new EnegenScheduleTradeDetail
                                {
                                    SettlementPeriod = index + 1,
                                    Volume = tuple.v,
                                    Price = tuple.p
                                }
                            }
                        })
                    };
                }
            }
        }

        private void InsertTrade(string environment, IEnumerable<EnegenInboundTrade> inboundTrades)
        {

        }
    }
}