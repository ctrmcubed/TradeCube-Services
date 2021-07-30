using Fidectus.Messages;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Serialization;
using Shared.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public class FidectusService : ApiService, IFidectusService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ApiService> logger;

        public FidectusService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<TradeConfirmationResponse> SendTradeConfirmation(TradeConfirmationRequest tradeConfirmationRequest, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                logger.LogInformation($"{TradeCubeJsonSerializer.Serialize(tradeConfirmationRequest)}");

                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusUrl);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {requestTokenResponse?.AccessToken}");
                httpClient.DefaultRequestHeaders.Add("CompanyId-Context", "60e70a7fb3fce11b7bfb438b");

                return await PostAsync<TradeConfirmationRequest, TradeConfirmationResponse>(httpClient, fidectusConfiguration.FidectusConfirmationUrl, tradeConfirmationRequest, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new TradeConfirmationResponse
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message
                };
            }
        }
    }
}
