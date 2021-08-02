using Fidectus.Messages;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public class FidectusAuthenticationService : ApiService, IFidectusAuthenticationService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ApiService> logger;

        public FidectusAuthenticationService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<RequestTokenResponse> FidectusGetAuthenticationToken(RequestTokenRequest requestTokenRequest, IFidectusConfiguration fidectusConfiguration)
        {
            var httpClient = httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusAuthUrl);

            var (response, httpResponse) = await PostAsync<RequestTokenRequest, RequestTokenResponse>(httpClient, fidectusConfiguration.FidectusAuthUrl, requestTokenRequest);

            logger.LogInformation($"FidectusGetAuthenticationToken: {httpResponse.IsSuccessStatusCode}");

            return response;
        }
    }
}
