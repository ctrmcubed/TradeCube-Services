using Equias.Messages;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Equias.Services
{
    public class EquiasAuthenticationService : ApiService, IEquiasAuthenticationService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ApiService> logger;

        public EquiasAuthenticationService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        public async Task<RequestTokenResponse> EboGetAuthenticationToken(RequestTokenRequest requestTokenRequest, IEquiasConfiguration equiasConfiguration)
        {
            var httpClient = httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);

            var (response, httpResponse) = await PostAsync<RequestTokenRequest, RequestTokenResponse>(httpClient, equiasConfiguration.RequestTokenUri, requestTokenRequest);

            logger.LogInformation($"EboGetAuthenticationToken: {httpResponse.IsSuccessStatusCode}");

            // Mutation!
            response.IsSuccessStatusCode = httpResponse.IsSuccessStatusCode;

            return response;
        }
    }
}
