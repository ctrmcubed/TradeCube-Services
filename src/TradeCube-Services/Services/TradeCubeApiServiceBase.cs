using System;
using System.Net.Http;
using TradeCube_Services.Configuration;

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

        protected HttpClient CreateClient(string apiJwtToken)
        {
            var client = clientFactory.CreateClient();

            client.BaseAddress = new Uri(tradeCubeConfiguration.WebApiUrl());
            client.DefaultRequestHeaders.Add("apiJwtToken", apiJwtToken);

            return client;
        }
    }
}