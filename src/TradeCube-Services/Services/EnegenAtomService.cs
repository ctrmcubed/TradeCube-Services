using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            var tradeList = trades.ToList();
            var apiResponseWrappers = await tradeProfileService.GetProfiles(tradeList, apiJwtToken).ToListAsync();

            var profileResponses = apiResponseWrappers
                .Where(d => d.Data != null && d.Data.Any())
                .SelectMany(p => p.Data);

            var inboundTrades = CreateInboundTrades(profileResponses, tradeList);

            logger.LogDebug(JsonSerializer.Serialize(inboundTrades));
        }

        private static IEnumerable<EnegenInboundTrade> CreateInboundTrades(IEnumerable<TradeProfileResponse> tradeProfileResponses, IEnumerable<TradeDataObject> trades)
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
            static IEnumerable<EnegenScheduleTrade> TradesOnDate(IEnumerable<TradeProfile> data, ILookup<(string TradeReference, int TradeLeg), TradeDataObject> lookup)
            {
                var byTrade = data
                    .GroupBy(p => (p.TradeReference, p.TradeLeg), p => p)
                    .Select(g => new { Trade = g.Key, Data = g.ToList() });

                return byTrade.Select(t => new EnegenScheduleTrade
                {
                    TradeId = HashHelper.HashStringToInteger($"{t.Trade.TradeReference}{t.Trade.TradeLeg}"),
                    Version = 1,
                    Counterparty = lookup[(t.Trade.TradeReference, t.Trade.TradeLeg)].SingleOrDefault()?.Counterparty?.Party,
                    ScheduleTradeDetails = t.Data.Select(t => new EnegenScheduleTradeDetail
                    {
                        SettlementPeriod = t.PeriodNumber,
                        Volume = t.Volume,
                        Price = t.Price
                    })
                });
            }

            var tradeLookup = trades.ToLookup(t => (t.TradeReference, t.TradeLeg), p => p);
            var combined = ProfileHelper.Combine(tradeProfileResponses, tradeLookup);

            var groupedByDate = combined
                .GroupBy(p => p.Local.Date, p => p)
                .Select(g => new { Date = g.Key, Data = g.ToList() })
                .OrderBy(d => d.Date);

            return groupedByDate.Select(g => new EnegenScheduleDate
            {
                Date = g.Date,
                ScheduleTrades = TradesOnDate(g.Data, tradeLookup)
            });
        }

        private void InsertTrade(string environment, IEnumerable<EnegenInboundTrade> inboundTrades)
        {
        }
    }
}