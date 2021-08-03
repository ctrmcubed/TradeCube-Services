using Microsoft.Extensions.Logging;
using Shared.Serialization;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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

        protected async Task<(TV response, HttpResponseMessage httpResponse)> GetAsync<TV>(HttpClient client, string uri, bool ensureSuccess = true)
        {
            try
            {
                var httpResponse = await client.GetAsync(uri);

                if (ensureSuccess)
                {
                    httpResponse.EnsureSuccessStatusCode();
                }

                await using var responseStream = await httpResponse.Content.ReadAsStreamAsync();

                var response = await TradeCubeJsonSerializer.DeserializeAsync<TV>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });

                return (response, httpResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected async Task<(TV response, HttpResponseMessage httpResponse)> PostAsync<T, TV>(HttpClient client, string uri, T body, bool ensureSuccess = true)
        {
            try
            {
                var httpResponse = await client.PostAsJsonAsync(uri, body, new JsonSerializerOptions { IgnoreNullValues = true });

                if (ensureSuccess)
                {
                    httpResponse.EnsureSuccessStatusCode();
                }

                await using var responseStream = await httpResponse.Content.ReadAsStreamAsync();

                var response = await TradeCubeJsonSerializer.DeserializeAsync<TV>(responseStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = false });

                return (response, httpResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected async Task<(TV deserializeAsync, HttpResponseMessage response)> PutAsync<T, TV>(HttpClient client, string uri, T body, bool ensureSuccess = true)
        {
            try
            {
                var httpResponse = await client.PutAsJsonAsync(uri, body, new JsonSerializerOptions { IgnoreNullValues = true });

                if (ensureSuccess)
                {
                    httpResponse.EnsureSuccessStatusCode();
                }

                await using var responseStream = await httpResponse.Content.ReadAsStreamAsync();

                var response = await TradeCubeJsonSerializer.DeserializeAsync<TV>(responseStream);

                return (response, httpResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(HttpClient client, string uri, T body, bool ensureSuccess = true)
        {
            try
            {
                var httpResponse = await client.PutAsJsonAsync(uri, body, new JsonSerializerOptions { IgnoreNullValues = true });

                if (ensureSuccess)
                {
                    httpResponse.EnsureSuccessStatusCode();
                }

                return httpResponse;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected async Task<TV> DeleteAsync<T, TV>(HttpClient client, string uri, T body, bool ensureSuccess = true)
        {
            static Uri ConstructUrl(string baseAddress, string uri)
            {
                return baseAddress.EndsWith("/") && uri.StartsWith("/")
                    ? new Uri($"{baseAddress}{uri.TrimStart('/')}")
                    : new Uri($"{baseAddress}{uri}");
            }

            try
            {
                // Standard DeleteAsync does not support sending a body
                var serializedBody = TradeCubeJsonSerializer.Serialize(body);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = ConstructUrl(client.BaseAddress?.ToString(), uri),
                    Content = new StringContent(serializedBody, Encoding.UTF8, "application/json")
                };

                var httpResponse = await client.SendAsync(request);

                if (ensureSuccess)
                {
                    httpResponse.EnsureSuccessStatusCode();
                }

                await using var responseStream = await httpResponse.Content.ReadAsStreamAsync();

                return await TradeCubeJsonSerializer.DeserializeAsync<TV>(responseStream);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
