using Fidectus.Messages;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Extensions;
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

        public FidectusService(IHttpClientFactory httpClientFactory, ILogger<FidectusService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<ConfirmationResponse> SendConfirmation(string method, string companyId, ConfirmationRequest confirmationRequest,
            RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration)
        {
            async Task<ConfirmationResponse> PostConfirmationAsync(HttpClient httpClient)
            {
                return await PostAsJsonAsync<ConfirmationRequest, ConfirmationResponse>(httpClient, fidectusConfiguration.FidectusConfirmationUrl, confirmationRequest, false);
            }

            async Task<ConfirmationResponse> PutConfirmationAsync(HttpClient httpClient)
            {
                return await PutAsJsonAsync<ConfirmationRequest, ConfirmationResponse>(httpClient, $"{fidectusConfiguration.FidectusConfirmationUrl}/{confirmationRequest.TradeConfirmation?.DocumentId}", confirmationRequest, false);
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
                logger.LogInformation("CompanyId-Context: '{CompanyId}', FidectusUrl: '{FidectusUrl}'",
                    companyId,
                    fidectusConfiguration.FidectusUrl);
                logger.JsonLogDebug("SendConfirmation Request: ", confirmationRequest);

                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusUrl);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {requestTokenResponse?.AccessToken}");
                httpClient.DefaultRequestHeaders.Add("CompanyId-Context", companyId);

                logger.LogInformation("FidectusSendTradeConfirmation, HTTP method: {Method}", method);

                var response = await HttpMethod(method, httpClient);

                logger.JsonLogDebug("SendConfirmation", response);

                // Mutation!
                response.IsSuccessStatusCode = response.IsSuccessStatusCode;
                response.StatusCode = response.Status;
                response.DocumentId = confirmationRequest.TradeConfirmation.DocumentId;

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return new ConfirmationResponse
                {
                    IsSuccessStatusCode = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ConfirmationResponse> DeleteConfirmation(string companyId, string docId, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient();

                httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusUrl);
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {requestTokenResponse?.AccessToken}");
                httpClient.DefaultRequestHeaders.Add("CompanyId-Context", companyId);

                var uri = $"{fidectusConfiguration.FidectusConfirmationUrl}/{docId}";
                var confirmationResponse = await DeleteAsync<ConfirmationResponse>(httpClient, uri, false);

                var response = new ConfirmationResponse
                {
                    IsSuccessStatusCode = confirmationResponse.IsSuccessStatusCode,
                    StatusCode = confirmationResponse.StatusCode,
                    Message = confirmationResponse.Message
                };

                logger.JsonLogDebug($"DeleteConfirmation {confirmationResponse.StatusCode}", response);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return new ConfirmationResponse();
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

                logger.JsonLogDebug($"GetBoxResult {httpResponse.StatusCode}", response);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return new BoxResultResponse();
            }
        }
    }
}
