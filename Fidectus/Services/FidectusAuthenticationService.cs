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

        public FidectusAuthenticationService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest, IFidectusConfiguration fidectusConfiguration)
        {
            var httpClient = httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(fidectusConfiguration.FidectusAuthUrl);

            return await PostAsync<RequestTokenRequest, RequestTokenResponse>(httpClient, fidectusConfiguration.FidectusAuthUrl, requestTokenRequest);
        }
    }
}
