using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Constants;
using Shared.Messages;
using Shared.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class TradeCubeApiService : ApiService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ITradeCubeConfiguration tradeCubeConfiguration;
        private readonly ILogger<ApiService> logger;

        protected TradeCubeApiService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration, ILogger<ApiService> logger) : base(logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.tradeCubeConfiguration = tradeCubeConfiguration;
            this.logger = logger;
        }

        protected async Task<ApiResponseWrapper<IEnumerable<T>>> GetViaApiKeyAsync<T>(string action, string apiKey, string queryString = null)
        {
            try
            {
                return await TradeCubeViaApiKeyAsync<ApiResponseWrapper<IEnumerable<T>>>(apiKey, action, queryString);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new ApiResponseWrapper<IEnumerable<T>>
                {
                    Message = ex.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Data = new List<T>()
                };
            }
        }

        protected async Task<ApiResponseWrapper<IEnumerable<T>>> GetViaJwtAsync<T>(string action, string apiJwtToken, string queryString = null)
        {
            try
            {
                return await TradeCubeGetViaJwtAsync<ApiResponseWrapper<IEnumerable<T>>>(apiJwtToken, action, queryString);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return new ApiResponseWrapper<IEnumerable<T>>
                {
                    Message = ex.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Data = new List<T>()
                };
            }
        }

        protected async Task<TV> TradeCubePostViaJwtAsync<T, TV>(string apiJwtToken, string action, T request) where TV : ApiResponse =>
            await PostAsync<T, TV>(CreateClientViaJwt(apiJwtToken), action, request);

        protected async Task<TV> TradeCubePostViaApiKeyAsync<T, TV>(string apiKey, string action, T request) where TV : ApiResponse =>
            await PostAsync<T, TV>(CreateClientViaApiKey(apiKey), action, request);

        protected async Task<TV> TradeCubePutViaApiKeyAsync<T, TV>(string apiKey, string action, T request) where TV : ApiResponse =>
            await PutAsync<T, TV>(CreateClientViaApiKey(apiKey), action, request);

        protected async Task<TV> TradeCubePutViaJwtAsync<T, TV>(string apiKey, string action, T request) where TV : ApiResponse =>
            await PutAsync<T, TV>(CreateClientViaJwt(apiKey), action, request);

        private async Task<TV> TradeCubeGetViaJwtAsync<TV>(string apiJwtToken, string action, string queryString = null) =>
            await GetAsync<TV>(CreateClientViaJwt(apiJwtToken), action, queryString);

        private async Task<TV> TradeCubeViaApiKeyAsync<TV>(string apiKey, string action, string queryString = null) =>
            await GetAsync<TV>(CreateClientViaApiKey(apiKey), action, queryString);

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
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private HttpClient CreateClientViaJwt(string apiJwtToken)
        {
            var client = httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(tradeCubeConfiguration.WebApiUrl());
            client.DefaultRequestHeaders.Add(ApiConstants.ApiJwtHeader, apiJwtToken);

            logger.LogDebug($"SJP Speciali, JWT: {apiJwtToken}");

            return client;
        }

        private HttpClient CreateClientViaApiKey(string apiKey)
        {
            var client = httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(tradeCubeConfiguration.WebApiUrl());
            client.DefaultRequestHeaders.Add(ApiConstants.ApiKeyHeader, apiKey);

            return client;
        }
    }
}
