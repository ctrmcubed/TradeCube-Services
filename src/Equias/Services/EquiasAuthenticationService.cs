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

        public EquiasAuthenticationService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<RequestTokenResponse> EboGetAuthenticationToken(RequestTokenRequest requestTokenRequest, IEquiasConfiguration equiasConfiguration)
        {
            var httpClient = httpClientFactory.CreateClient();

            httpClient.BaseAddress = new Uri(equiasConfiguration.ApiDomain);

            return await PostAsync<RequestTokenRequest, RequestTokenResponse>(httpClient, equiasConfiguration.RequestTokenUri, requestTokenRequest);
        }
    }
}
