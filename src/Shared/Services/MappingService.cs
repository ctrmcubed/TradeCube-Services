using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class MappingService : TradeCubeApiService, IMappingService
    {
        public MappingService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<MappingService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingViaApiKeyAsync(string mappingKey, string mappingFrom, string apiKey)
        {
            return await GetViaApiKeyAsync<MappingDataObject>($"Mapping/{mappingKey}/{mappingFrom}", apiKey);
        }

        public async Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingsViaApiKeyAsync(string apiKey)
        {
            return await GetViaApiKeyAsync<MappingDataObject>("Mapping", apiKey);
        }

        public async Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingViaJwtAsync(string mappingKey, string mappingFrom, string apiJwtToken)
        {
            return await GetViaJwtAsync<MappingDataObject>($"Mapping/{mappingKey}/{mappingFrom}", apiJwtToken);
        }

        public async Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingsViaJwtAsync(string apiJwtToken)
        {
            return await GetViaJwtAsync<MappingDataObject>("Mapping", apiJwtToken);
        }
    }
}
