using Microsoft.Extensions.Logging;
using Shared.Messages;
using Shared.Serialization;
using System;
using System.IO;
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

        protected async Task<TV> PostAsync<T, TV>(HttpClient client, string action, T body, bool ensureSuccess = true) where TV : ApiResponse, new()
        {
            try
            {
                var response = await client.PostAsJsonAsync(action, body, new JsonSerializerOptions { IgnoreNullValues = true });

                if (ensureSuccess)
                {
                    response.EnsureSuccessStatusCode();
                }

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                var deserializeAsync = await DeserializeAsync<TV>(responseStream);

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

        protected async Task<TV> PutAsync<T, TV>(HttpClient client, string action, T body, bool ensureSuccess = true) where TV : ApiResponse, new()
        {
            try
            {
                var response = await client.PutAsJsonAsync(action, body, new JsonSerializerOptions { IgnoreNullValues = true });

                if (ensureSuccess)
                {
                    response.EnsureSuccessStatusCode();
                }

                await using var responseStream = await response.Content.ReadAsStreamAsync();

                var deserializeAsync = await DeserializeAsync<TV>(responseStream);

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

        protected async Task<TV> DeleteAsync<T, TV>(HttpClient client, string action, T body, bool ensureSuccess = true) where TV : ApiResponse
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
                    RequestUri = ConstructUrl(client.BaseAddress?.ToString(), action),
                    Content = new StringContent(serializedBody, Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);

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

        protected async Task<HttpResponseMessage> DeleteAsync<T>(HttpClient client, string action, T body, bool ensureSuccess = true)
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

                var serializedBody = body is null
                    ? null
                    : TradeCubeJsonSerializer.Serialize(body);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = ConstructUrl(client.BaseAddress?.ToString(), action),
                    Content = serializedBody is null
                        ? null
                        : new StringContent(serializedBody, Encoding.UTF8, "application/json")
                };

                var response = await client.SendAsync(request);

                if (ensureSuccess)
                {
                    response.EnsureSuccessStatusCode();
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<TV> DeserializeAsync<TV>(Stream stream) where TV : new()
        {
            try
            {
                return await TradeCubeJsonSerializer.DeserializeAsync<TV>(stream) ?? default;
            }
            catch (Exception ex)
            {
                logger.LogError($"Could not parse json response ({ex.Message})");
                return new TV();
            }
        }
    }
}
