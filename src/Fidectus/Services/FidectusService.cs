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

                var uri = $"{fidectusConfiguration.FidectusConfirmationUrl}?docIds={string.Join(",", docIds)}";
                var (response, httpResponse) = await GetAsync<GetConfirmationResponse>(httpClient, uri, false);

                logger.LogInformation($"GetTradeConfirmation: {httpResponse.StatusCode}");

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new GetConfirmationResponse();
            }
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

        public async Task<SendConfirmationResponse> SendTradeConfirmation(string method, string companyId, TradeConfirmationRequest tradeConfirmationRequest,
            RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration)
        {
            async Task<SendConfirmationResponse> PostConfirmationAsync(HttpClient httpClient)
            {
                return await PostAsync<TradeConfirmationRequest, SendConfirmationResponse>(httpClient, fidectusConfiguration.FidectusConfirmationUrl, tradeConfirmationRequest, false);
            }

            async Task<SendConfirmationResponse> PutConfirmationAsync(HttpClient httpClient)
            {
                var url = $"{fidectusConfiguration.FidectusConfirmationUrl}/{tradeConfirmationRequest.TradeConfirmation?.DocumentId}";

                return await PutAsync<TradeConfirmationRequest, SendConfirmationResponse>(httpClient, url, tradeConfirmationRequest, false);
            }

            async Task<SendConfirmationResponse> HttpMethod(string httpMethod, HttpClient httpClient)
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
                return new SendConfirmationResponse
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message
                };
            }
        }
    }
}
