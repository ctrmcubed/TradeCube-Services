using System;
using System.Net.Http;
using TradeCube_Services.Configuration;
using TradeCube_Services.Constants;

namespace TradeCube_Services.Services
{
    public class TradeCubeApiServiceBase
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly ITradeCubeConfiguration tradeCubeConfiguration;

        protected TradeCubeApiServiceBase(IHttpClientFactory clientFactory, ITradeCubeConfiguration tradeCubeConfiguration)
        {
            this.clientFactory = clientFactory;
            this.tradeCubeConfiguration = tradeCubeConfiguration;
        }

        protected HttpClient CreateClientViaJwt(string apiJwtToken)
        {
            var client = clientFactory.CreateClient();

            client.BaseAddress = new Uri(tradeCubeConfiguration.WebApiUrl());
            client.DefaultRequestHeaders.Add(ApiConstants.ApiJwtHeader, apiJwtToken);

            return client;
        }

        protected HttpClient CreateClientViaApiKey(string apiKey)
        {
            var client = clientFactory.CreateClient();

            client.BaseAddress = new Uri(tradeCubeConfiguration.WebApiUrl());
            client.DefaultRequestHeaders.Add(ApiConstants.ApiKeyHeader, apiKey);

            return client;
        }
    }
}