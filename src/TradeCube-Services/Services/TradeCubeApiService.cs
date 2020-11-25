using AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TradeCube_Services.Configuration;
using TradeCube_Services.Messages;
using TradeCube_Services.Serialization;

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

        protected async Task<ApiResponseWrapper<IEnumerable<T>>> GetViaApiKeyAsync<T>(string action, string apiKey)
        {
            try
            {
                return await TradeCubeViaApiKeyAsync<ApiResponseWrapper<IEnumerable<T>>>(apiKey, action);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<IEnumerable<T>>
                {
                    Message = e.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Data = new List<T>()
                };
            }
        }

        protected async Task<ApiResponseWrapper<IEnumerable<T>>> GetViaJwtAsync<T>(string action, string apiJwtToken)
        {
            try
            {
                return await TradeCubeGetViaJwtAsync<ApiResponseWrapper<IEnumerable<T>>>(apiJwtToken, action);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<IEnumerable<T>>
                {
                    Message = e.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Data = new List<T>()
                };
            }
        }

        protected async Task<TV> TradeCubePostViaJwtAsync<T, TV>(string apiJwtToken, string action, T request) =>
            await PostAsync<T, TV>(CreateClientViaJwt(apiJwtToken), action, request);

        protected async Task<TV> TradeCubePostViaApiKeyAsync<T, TV>(string apiKey, string action, T request) =>
            await PostAsync<T, TV>(CreateClientViaApiKey(apiKey), action, request);

        private async Task<TV> TradeCubeGetViaJwtAsync<TV>(string apiJwtToken, string action, string queryString = null) =>
            await GetAsync<TV>(CreateClientViaJwt(apiJwtToken), action);

        private async Task<TV> TradeCubeViaApiKeyAsync<TV>(string apiKey, string action, string queryString = null) =>
            await GetAsync<TV>(CreateClientViaApiKey(apiKey), action);

        private async Task<TV> GetAsync<TV>(HttpClient client, string action, string queryString = null)
        {
            try
            {
                var requestUri = string.IsNullOrEmpty(queryString)
                    ? $"{action}"
                    : $"{action}/{queryString}";

                var response = await client.GetAsync(requestUri);

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

        private async Task<TV> PostAsync<T, TV>(HttpClient client, string action, T request)
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
