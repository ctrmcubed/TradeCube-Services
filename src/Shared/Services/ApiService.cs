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

        protected async Task<(TV response, HttpResponseMessage httpResponse)> GetAsync<TV>(HttpClient client, string uri, bool ensureSuccess = true) where TV : ApiResponse, new()
        {
            try
            {
                var httpResponseMessage = await client.GetAsync(uri);

                if (ensureSuccess)
                {
                    httpResponseMessage.EnsureSuccessStatusCode();
                }

                await using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                var response = await DeserializeAsync<TV>(responseStream);

                return (response, httpResponseMessage);
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
                var httpResponseMessage = await client.PostAsJsonAsync(action, body, new JsonSerializerOptions { IgnoreNullValues = true });

                if (ensureSuccess)
                {
                    httpResponseMessage.EnsureSuccessStatusCode();
                }

                await using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                var response = await DeserializeAsync<TV>(responseStream);

                response.StatusCode = (int?)httpResponseMessage.StatusCode;
                response.IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
                response.Message = httpResponseMessage.ReasonPhrase;

                return response;
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
                var httpResponseMessage = await client.PutAsJsonAsync(action, body, new JsonSerializerOptions { IgnoreNullValues = true });

                if (ensureSuccess)
                {
                    httpResponseMessage.EnsureSuccessStatusCode();
                }

                await using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                var response = await DeserializeAsync<TV>(responseStream);

                response.StatusCode = (int?)httpResponseMessage.StatusCode;
                response.IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
                response.Message = httpResponseMessage.ReasonPhrase;

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        protected async Task<TV> DeleteAsync<TV>(HttpClient client, string action, bool ensureSuccess = true) where TV : ApiResponse, new()
        {
            static Uri ConstructUrl(string baseAddress, string uri)
            {
                return baseAddress.EndsWith("/") && uri.StartsWith("/")
                    ? new Uri($"{baseAddress}{uri.TrimStart('/')}")
                    : new Uri($"{baseAddress}{uri}");
            }

            try
            {
                var constructUrl = ConstructUrl(client.BaseAddress?.ToString(), action);
                var httpResponseMessage = await client.DeleteAsync(constructUrl);

                if (ensureSuccess)
                {
                    httpResponseMessage.EnsureSuccessStatusCode();
                }

                await using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                var response = await DeserializeAsync<TV>(responseStream);

                response.StatusCode = (int?)httpResponseMessage.StatusCode;
                response.IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
                response.Message = httpResponseMessage.ReasonPhrase;

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }


        protected async Task<TV> DeleteAsync<T, TV>(HttpClient client, string action, T body, bool ensureSuccess = true) where TV : ApiResponse, new()
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

                var httpResponseMessage = await client.SendAsync(request);

                if (ensureSuccess)
                {
                    httpResponseMessage.EnsureSuccessStatusCode();
                }

                await using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                var response = await DeserializeAsync<TV>(responseStream);

                response.StatusCode = (int?)httpResponseMessage.StatusCode;
                response.IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode;
                response.Message = httpResponseMessage.ReasonPhrase;

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
                return await TradeCubeJsonSerializer.DeserializeAsync<TV>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = false }) ?? default;
            }
            catch (Exception ex)
            {
                logger.LogError($"Could not parse json response ({ex.Message})");
                return new TV();
            }
        }
    }
}
