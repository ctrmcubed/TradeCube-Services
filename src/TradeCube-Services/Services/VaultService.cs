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
    public class VaultService : TradeCubeApiService, IVaultService
    {
        private readonly ILogger<TradeCubeApiService> logger;

        public VaultService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration, ILogger<VaultService> logger) :
            base(httpClientFactory, tradeCubeConfiguration, logger)
        {
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<IEnumerable<VaultDataObject>>> Vault(string apiJwtToken, string key)
        {
            try
            {
                return await Get<ApiResponseWrapper<IEnumerable<VaultDataObject>>>(apiJwtToken, $"Vault/{key}");
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<IEnumerable<VaultDataObject>> { Message = e.Message, Status = HttpStatusCode.BadRequest.ToString() };
            }
        }
    }
}