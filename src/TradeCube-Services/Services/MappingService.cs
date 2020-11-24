using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TradeCube_Services.Configuration;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public class MappingService : TradeCubeApiService, IMappingService
    {
        private readonly ILogger<MappingService> logger;

        public MappingService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<MappingService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<IEnumerable<MappingDataObject>>> GetMappingAsync(string mappingKey, string mappingFrom, string apiKey)
        {
            try
            {
                return await GetViaApiKeyAsync<ApiResponseWrapper<IEnumerable<MappingDataObject>>>(apiKey, $"Mapping/{mappingKey}/{mappingFrom}");
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<IEnumerable<MappingDataObject>>
                {
                    Message = e.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Data = new List<MappingDataObject>()
                };
            }
        }
    }
}
