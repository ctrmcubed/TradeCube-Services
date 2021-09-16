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

        public async Task<BoxResultResponse> GetBoxResult(string companyId, string docId, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusUrl);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {requestTokenResponse?.AccessToken}");
                httpClient.DefaultRequestHeaders.Add("CompanyId-Context", companyId);

                var uri = $"{fidectusConfiguration.FidectusConfirmationBoxResultUrl}/{docId}";
                var (response, httpResponse) = await GetAsync<BoxResultResponse>(httpClient, uri, false);

                logger.LogInformation($"GetBoxResult: {httpResponse.StatusCode}");

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new BoxResultResponse();
            }
        }

        public async Task<ConfirmationResponse> SendTradeConfirmation(string method, string companyId, TradeConfirmationRequest tradeConfirmationRequest,
            RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration)
        {
            async Task<ConfirmationResponse> PostConfirmationAsync(HttpClient httpClient)
            {
                var httpResponseMessage = await PostAsync(httpClient, fidectusConfiguration.FidectusConfirmationUrl, tradeConfirmationRequest, false);

                return new ConfirmationResponse
                {
                    IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
                    StatusCode = (int)httpResponseMessage.StatusCode
                };
            }

            async Task<ConfirmationResponse> PutConfirmationAsync(HttpClient httpClient)
            {
                var httpResponseMessage = await PutAsync(httpClient, $"{fidectusConfiguration.FidectusConfirmationUrl}/{tradeConfirmationRequest.TradeConfirmation?.DocumentId}", tradeConfirmationRequest, false);

                return new ConfirmationResponse
                {
                    IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
                    StatusCode = (int)httpResponseMessage.StatusCode
                };
            }

            async Task<ConfirmationResponse> HttpMethod(string httpMethod, HttpClient httpClient)
            {
                return httpMethod switch
                {
                    "POST" => await PostConfirmationAsync(httpClient),
                    "PUT" => await PutConfirmationAsync(httpClient),
                    _ => throw new Exception($"Unknown HTTP method ({method})")
                };
            }

            try
            {
                logger.LogInformation($"CompanyId: {companyId}");
                logger.LogInformation($"{TradeCubeJsonSerializer.Serialize(tradeConfirmationRequest)}");

                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusUrl);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {requestTokenResponse?.AccessToken}");
                httpClient.DefaultRequestHeaders.Add("CompanyId-Context", companyId);

                logger.LogInformation($"FidectusSendTradeConfirmation, HTTP method: {method}");

                var response = await HttpMethod(method, httpClient);

                logger.LogInformation($"FidectusSendTradeConfirmation: {response.Status}");

                // Mutation!
                response.IsSuccessStatusCode = response.IsSuccessStatusCode;

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new ConfirmationResponse
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message
                };
            }
        }
    }
}
