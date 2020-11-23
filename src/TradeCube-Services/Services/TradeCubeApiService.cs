using AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TradeCube_Services.Configuration;

namespace TradeCube_Services.Services
{
    public class TradeCubeApiService : TradeCubeApiServiceBase
    {
        private readonly ILogger<TradeCubeApiService> logger;

        protected TradeCubeApiService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration, ILogger<TradeCubeApiService> logger)
            : base(httpClientFactory, tradeCubeConfiguration)
        {
            this.logger = logger;
        }

        protected async Task<TV> GetViaJwt<TV>(string apiJwtToken, string action, string queryString = null) =>
            await Get<TV>(CreateClientViaJwt(apiJwtToken), action);

        protected async Task<TV> GetViaApiKey<TV>(string apiKey, string action, string queryString = null) =>
            await Get<TV>(CreateClientViaApiKey(apiKey), action);

        protected async Task<TV> PostViaJwt<TV>(string apiJwtToken, string action, JObject request) =>
            await Post<TV>(CreateClientViaJwt(apiJwtToken), request, action);

        protected async Task<TV> PostViaApiKey<TV>(string apiKey, string action, JObject request) =>
            await Post<TV>(CreateClientViaApiKey(apiKey), request, action);

        private async Task<TV> Get<TV>(HttpClient client, string action, string queryString = null)
        {
            try
            {
                var requestUri = string.IsNullOrEmpty(queryString)
                    ? $"{action}"
                    : $"{action}/{queryString}";

                var response = await client.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<TV>(responseStream);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        private async Task<TV> Post<TV>(HttpClient client, JObject request, string action)
        {
            try
            {
                var response = await client.PostAsJsonAsync(action, request);

                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<TV>(responseStream);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
