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
    public class PartyService : TradeCubeApiService, IPartyService
    {
        private readonly ILogger<TradeCubeApiService> logger;

        public PartyService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
            this.logger = logger;
        }
        public async Task<ApiResponseWrapper<IEnumerable<PartyDataObject>>> GetPartyAsync(string party, string apiKey)
        {
            try
            {
                return await GetViaApiKeyAsync<ApiResponseWrapper<IEnumerable<PartyDataObject>>>(apiKey, $"Party/{party}");
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<IEnumerable<PartyDataObject>>
                {
                    Message = e.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Data = new List<PartyDataObject>()
                };
            }
        }
    }
}