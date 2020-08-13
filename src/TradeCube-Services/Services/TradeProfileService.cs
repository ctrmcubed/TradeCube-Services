using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TradeCube_Services.Configuration;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public class TradeProfileService : TradeCubeApiService, ITradeProfileService
    {
        private readonly ILogger<TradeProfileService> logger;

        public TradeProfileService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration, ILogger<TradeProfileService> logger)
            : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradeProfileResponse>>> TradeProfiles(string apiJwtToken, TradeProfileRequest tradeProfileRequest)
        {
            try
            {
                var url = $"Profile/{tradeProfileRequest.TradeReference}?TradeLeg={tradeProfileRequest.TradeLeg}&ProfileFormat={tradeProfileRequest.ProfileFormat}&Volume={tradeProfileRequest.Volume}&Price={tradeProfileRequest.Price}";

                return await Get<ApiResponseWrapper<IEnumerable<TradeProfileResponse>>>(apiJwtToken, url);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<IEnumerable<TradeProfileResponse>> { Message = e.Message, Status = HttpStatusCode.BadRequest.ToString() };
            }
        }

        public async IAsyncEnumerable<ApiResponseWrapper<IEnumerable<TradeProfileResponse>>> GetProfiles(IEnumerable<TradeDataObject> trades, string apiJwtToken)
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

                yield return await TradeProfiles(apiJwtToken, tradeProfileRequest);
            }
        }
    }
}