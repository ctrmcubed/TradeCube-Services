﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services
{
    public class TradeService : TradeCubeApiService, ITradeService
    {
        private readonly ILogger<TradeCubeApiService> logger;

        public TradeService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration, 
            ILogger<TradeService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> GetTradeAsync(string apiJwtToken, string tradeReference, int tradeLeg)
        {
            try
            {
                var trade = await GetViaJwtAsync<TradeDataObject>("Trade", apiJwtToken, $"{tradeReference}?TradeLeg={tradeLeg}");

                return trade is null
                    ? new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                    {
                        Message = "Trade not found",
                        Status = HttpStatusCode.BadRequest.ToString(),
                        StatusCode = (int?)HttpStatusCode.BadRequest
                    }
                    : new ApiResponseWrapper<IEnumerable<TradeDataObject>> { Data = trade.Data };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                {
                    Message = ex.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    StatusCode = (int?)HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> GetTradesAsync(string apiJwtToken, TradeRequest tradeRequest)
        {
            try
            {
                var query = new JObject
                {
                    new JProperty("TradeReference", new JObject(new JProperty("$in", new JArray(tradeRequest.TradeReferences))))
                };
                
                return await TradeCubePostAsStringViaJwtAsync<ApiResponseWrapper<IEnumerable<TradeDataObject>>>(apiJwtToken, "Trade/query", query);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                {
                    Message = ex.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    StatusCode = (int?)HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> PostTradesViaApiKeyAsync(string apiKey, IEnumerable<TradeDataObject> trades)
        {
            try
            {
                return await TradeCubePostViaApiKeyAsync<ApiRequest<IEnumerable<TradeDataObject>>,
                    ApiResponseWrapper<IEnumerable<TradeDataObject>>>(apiKey, "Trade/Merge", new ApiRequest<IEnumerable<TradeDataObject>>(trades));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                {
                    Message = ex.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    StatusCode = (int?)HttpStatusCode.BadRequest
                };
            }
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> PutTradesViaJwtAsync(string apiJwtToken, IEnumerable<TradeDataObject> trades)
        {
            try
            {
                return await TradeCubePutViaJwtAsync<ApiRequest<IEnumerable<TradeDataObject>>,
                    ApiResponseWrapper<IEnumerable<TradeDataObject>>>(apiJwtToken, "Trade/Merge", new ApiRequest<IEnumerable<TradeDataObject>>(trades));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                {
                    Message = ex.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    StatusCode = (int?)HttpStatusCode.BadRequest
                };
            }
        }
    }
}