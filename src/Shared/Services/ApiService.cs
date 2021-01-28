using Microsoft.Extensions.Logging;
using Shared.Messages;
using Shared.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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

        protected async Task<TV> PostAsync<T, TV>(HttpClient client, string action, T request, bool ensureSuccess = true) where TV : ApiResponse
        {
            try
            {
                var response = await client.PostAsJsonAsync(action, request, new JsonSerializerOptions { IgnoreNullValues = true });

                if (ensureSuccess)
                {
                    response.EnsureSuccessStatusCode();
                }

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                var deserializeAsync = await TradeCubeJsonSerializer.DeserializeAsync<TV>(responseStream);

                deserializeAsync.Status = response.StatusCode.ToString();
                deserializeAsync.IsSuccessStatusCode = response.IsSuccessStatusCode;
                deserializeAsync.Message = response.ReasonPhrase;

                return deserializeAsync;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected async Task<TV> PutAsync<T, TV>(HttpClient client, string action, T request, bool ensureSuccess = true) where TV : ApiResponse
        {
            try
            {
                var response = await client.PutAsJsonAsync(action, request, new JsonSerializerOptions { IgnoreNullValues = true });

                if (ensureSuccess)
                {
                    response.EnsureSuccessStatusCode();
                }

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                var deserializeAsync = await TradeCubeJsonSerializer.DeserializeAsync<TV>(responseStream);

                deserializeAsync.Status = response.StatusCode.ToString();
                deserializeAsync.IsSuccessStatusCode = response.IsSuccessStatusCode;
                deserializeAsync.Message = response.ReasonPhrase;

                return deserializeAsync;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
