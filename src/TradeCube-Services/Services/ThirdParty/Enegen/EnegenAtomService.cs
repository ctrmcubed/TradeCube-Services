using Microsoft.Extensions.Logging;
using RestSharp;
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

namespace TradeCube_Services.Services.ThirdParty.Enegen
{
    public class EnegenAtomService : IEnegenAtomService
    {
        private readonly ITradeService tradeService;
        private readonly ITradeProfileService tradeProfileService;
        private readonly IVaultService vaultService;
        private readonly ILogger<EnegenAtomService> logger;

        public EnegenAtomService(ITradeService tradeService, ITradeProfileService tradeProfileService, IVaultService vaultService, ILogger<EnegenAtomService> logger)
        {
            this.tradeService = tradeService;
            this.tradeProfileService = tradeProfileService;
            this.vaultService = vaultService;
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
                    var enegenInboundTrades = await ProcessTrades(trades.Data, apiJwtToken);
                    var (username, password) = await AtomCredentials(apiJwtToken);
                    var atomBearerToken = AtomBearerToken(enegenAtomTradeParameters.Url, username, password);
                    var json = JsonSerializer.Serialize(enegenInboundTrades);

                    logger.LogDebug($"Request: {json}");

                    var response = PostTrades(enegenAtomTradeParameters.Url, atomBearerToken, json);

                    logger.LogDebug($"Response: {response.Content}");

                    return new ApiResponseWrapper<WebServiceResponse>
                    {
                        Status = response.IsSuccessful
                            ? ApiConstants.SuccessResult
                            : ApiConstants.FailedResult,
                        Message = "Success",
                        Data = new WebServiceResponse
                        {
                            WebService = enegenAtomTradeParameters.WebService
                        }
                    };
                }

                logger.LogError("Error calling Trade API", trades.Message);
                return new ApiResponseWrapper<WebServiceResponse>
                {
                    Status = ApiConstants.FailedResult,
                    Message = trades.Message,
                    Data = new WebServiceResponse
                    {
                        WebService = enegenAtomTradeParameters.WebService
                    }
                };
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<WebServiceResponse>
                {
                    Status = ApiConstants.FailedResult,
                    Message = e.Message,
                    Data = new WebServiceResponse
                    {
                        WebService = enegenAtomTradeParameters.WebService
                    }
                };
            }
        }

        private async Task<(string username, string password)> AtomCredentials(string apiJwtToken)
        {
            var username = await vaultService.Vault(apiJwtToken, "AtomUsername");
            var password = await vaultService.Vault(apiJwtToken, "AtomPassword");

            return (username?.Data?.FirstOrDefault()?.VaultValue, password?.Data?.FirstOrDefault()?.VaultValue);
        }

        private static string AtomBearerToken(string url, string username, string password)
        {
            var client = new RestClient($"{url}/token?Username={username}&Password={password}");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Length", "0");

            var response = client.Execute(request);

            // Trim response, slight fudge because the response is quoted, for some reason
            return response.Content
                .TrimStart('"')
                .TrimEnd('"');
        }

        private async Task<IEnumerable<EnegenInboundTrade>> ProcessTrades(IEnumerable<TradeDataObject> trades, string apiJwtToken)
        {
            var tradeList = trades.ToList();
            var apiResponseWrappers = await tradeProfileService.GetProfiles(tradeList, apiJwtToken).ToListAsync();

            var profileResponses = apiResponseWrappers
                .Where(d => d.Data != null && d.Data.Any())
                .SelectMany(p => p.Data);

            return CreateInboundTrades(profileResponses, tradeList);
        }

        private static IRestResponse PostTrades(string url, string bearerToken, string inboundTrades)
        {
            var client = new RestClient($"{url}/Trades");
            var request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {bearerToken}");
            request.AddParameter("application/json", inboundTrades, ParameterType.RequestBody);

            var response = client.Execute(request);

            return response;
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
                    .Select(g => new
                    {
                        g.Key,
                        Data = g.ToList(),
                        Trade = lookup[(g.Key.TradeReference, g.Key.TradeLeg)].SingleOrDefault()
                    });

                return byTrade.Select(t => new EnegenScheduleTrade
                {
                    TradeId = HashHelper.HashStringToInteger($"{t.Key.TradeReference}{t.Key.TradeLeg}"),
                    Version = 1,
                    Product = t.Trade?.Product?.Product,
                    Counterparty = t.Trade?.Counterparty?.Party,
                    Trader = t.Trade?.InternalTrader?.ContactLongName,
                    ScheduleTradeDetails = t.Data.Select(d => new EnegenScheduleTradeDetail
                    {
                        SettlementPeriodStartTime = $"{d.Utc.Hour:D2}:{d.Utc.Minute:D2}:{d.Utc.Second:D2}",
                        Volume = d.Volume,
                        Price = d.Price
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
    }
}