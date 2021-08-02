using Fidectus.Messages;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Serialization;
using Shared.Services;
using System;
using System.Collections.Generic;
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

        public async Task<GetConfirmationResponse> GetTradeConfirmation(string companyId, IEnumerable<string> docIds, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusUrl);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {requestTokenResponse?.AccessToken}");
                httpClient.DefaultRequestHeaders.Add("CompanyId-Context", companyId);

                var uri = $"{fidectusConfiguration.FidectusConfirmationUrl}?docIds={docIds}";

                var (response, httpResponse) = await GetAsync<GetConfirmationResponse>(httpClient, uri, false);

                // Mutation!
                response.IsSuccessStatusCode = httpResponse.IsSuccessStatusCode;

                // Other HttpResponse fields not needed

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new GetConfirmationResponse
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message
                };
            }
        }


        public async Task<SendConfirmationResponse> SendTradeConfirmation(string companyId,
            TradeConfirmationRequest tradeConfirmationRequest, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                logger.LogInformation($"CompanyId: {companyId}");
                logger.LogInformation($"{TradeCubeJsonSerializer.Serialize(tradeConfirmationRequest)}");

                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusUrl);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {requestTokenResponse?.AccessToken}");
                httpClient.DefaultRequestHeaders.Add("CompanyId-Context", companyId);

                var (response, httpResponse) = await PostAsync<TradeConfirmationRequest, SendConfirmationResponse>(httpClient, fidectusConfiguration.FidectusConfirmationUrl, tradeConfirmationRequest, false);

                logger.LogInformation($"FidectusSendTradeConfirmation: {httpResponse.StatusCode}");

                // Mutation!
                response.IsSuccessStatusCode = httpResponse.IsSuccessStatusCode;

                // Other HttpResponse fields not needed

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new SendConfirmationResponse
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message
                };
            }
        }
    }
}
