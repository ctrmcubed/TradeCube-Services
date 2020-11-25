using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TradeCube_Services.Configuration;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public class MappingService : TradeCubeApiService, IMappingService
    {
        public MappingService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<MappingService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingAsync(string mappingKey, string mappingFrom, string apiKey)
        {
            return await GetViaApiKeyAsync<MappingDataObject>($"Mapping/{mappingKey}/{mappingFrom}", apiKey);
        }

        public async Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingsAsync(string apiKey)
        {
            return await GetViaApiKeyAsync<MappingDataObject>("Mapping", apiKey);
        }
    }
}
