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
        private readonly IEquiasConfiguration equiasConfiguration;

        public EquiasAuthenticationService(IHttpClientFactory httpClientFactory, IEquiasConfiguration equiasConfiguration, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.equiasConfiguration = equiasConfiguration;
        }

        public async Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest)
        {
            var httpClient = httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);

            return await PostAsync<RequestTokenRequest, RequestTokenResponse>(httpClient, equiasConfiguration.RequestTokenUri, requestTokenRequest);
        }
    }
}
