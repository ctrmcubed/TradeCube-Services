using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class TradeService : TradeCubeApiService, ITradeService
    {
        private readonly ILogger<TradeCubeApiService> logger;

        public TradeService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> GetTradesAsync(string apiJwtToken, TradeRequest tradeRequest)
        {
            try
            {
                var query = new JObject
                {
                    new JProperty("TradeReference", new JObject(new JProperty("$in", new JArray(tradeRequest.TradeReferences))))
                };

                return await TradeCubePostViaJwtAsync<JObject, ApiResponseWrapper<IEnumerable<TradeDataObject>>>(apiJwtToken, "Trade/query", query);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                {
                    Message = e.Message,
                    Status = HttpStatusCode.BadRequest.ToString()
                };
            }
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> SaveTradesAsync(string apiKey, IEnumerable<TradeDataObject> trades)
        {
            try
            {
                return await TradeCubePostViaApiKeyAsync<ApiRequest<IEnumerable<TradeDataObject>>,
                    ApiResponseWrapper<IEnumerable<TradeDataObject>>>(apiKey, "Trade", new ApiRequest<IEnumerable<TradeDataObject>>(trades));
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                {
                    Message = e.Message,
                    Status = HttpStatusCode.BadRequest.ToString()
                };
            }
        }
    }
}