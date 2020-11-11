using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TradeCube_Services.Constants;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;
using TradeCube_Services.Parameters;
using TradeCube_Services.ThirdParty.Enegen;

namespace TradeCube_Services.Services.ThirdParty.Enegen
{
    public class EnegenGenstarService : IEnegenGenstarService
    {
        private readonly ITradeService tradeService;
        private readonly ITradeProfileService tradeProfileService;
        private readonly ILogger<EnegenGenstarService> logger;

        public EnegenGenstarService(ITradeService tradeService, ITradeProfileService tradeProfileService, ILogger<EnegenGenstarService> logger)
        {
            this.tradeService = tradeService;
            this.tradeProfileService = tradeProfileService;
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<WebServiceResponse>> Trade(EnegenGenstarTradeParameters enegenGenstarTradeParameters)
        {
            try
            {
                var apiJwtToken = enegenGenstarTradeParameters.ApiJwtToken;
                var request = new TradeRequest { TradeReferences = enegenGenstarTradeParameters.TradeReferences };
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

            var genstarCsvData = CreateGenstarCsvData(profileResponses, tradeList);
            var genstarFileStructure = genstarCsvData.Select(f => f.CreateFileStructure());

            logger.LogDebug(JsonSerializer.Serialize(genstarFileStructure));
        }

        private static IEnumerable<EnegenGenstarFile> CreateGenstarCsvData(IEnumerable<TradeProfileResponse> tradeProfileResponses, IEnumerable<TradeDataObject> trades)
        {
            // TODO awaiting detail from Enegen
            return new List<EnegenGenstarFile>();
        }
    }
}