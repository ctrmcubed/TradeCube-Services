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

        protected async Task<TV> Get<TV>(string apiJwtToken, string action)
        {
            try
            {
                var client = CreateClient(apiJwtToken);
                var response = await client.GetAsync(action);

                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                logger.LogDebug(responseStream.ToString());

                return await JsonSerializer.DeserializeAsync<TV>(responseStream);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        protected async Task<TV> Post<TV>(string apiJwtToken, string action, JObject request)
        {
            try
            {
                var client = CreateClient(apiJwtToken);

                logger.LogInformation($"POST: BaseAddress={client.BaseAddress}, Action={action}");

                var response = await client.PostAsJsonAsync(action, request);

                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                logger.LogInformation(responseStream.ToString());

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
