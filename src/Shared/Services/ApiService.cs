using Microsoft.Extensions.Logging;
using Shared.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class ApiService
    {
        private readonly ILogger<ApiService> logger;

        protected ApiService(ILogger<ApiService> logger)
        {
            this.logger = logger;
        }

        public async Task<TV> PostAsync<T, TV>(HttpClient client, string action, T request)
        {
            try
            {
                var response = await client.PostAsJsonAsync(action, request);

                response.EnsureSuccessStatusCode();

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                return await TradeCubeJsonSerializer.DeserializeAsync<TV>(responseStream);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
