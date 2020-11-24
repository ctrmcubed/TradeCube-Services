using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TradeCube_Services.Configuration;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public class FingerprintService : TradeCubeApiService, IFingerprintService
    {
        private readonly ILogger<TradeCubeApiService> logger;

        public FingerprintService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<IEnumerable<FingerprintResponse>>> FingerprintAsync(string apiKey, FingerprintRequest fingerprintRequest)
        {
            try
            {
                return await PostViaApiKeyAsync<FingerprintRequest, ApiResponseWrapper<IEnumerable<FingerprintResponse>>>(apiKey, "Fingerprint", fingerprintRequest);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<IEnumerable<FingerprintResponse>>
                {
                    Message = e.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Data = new List<FingerprintResponse>()
                };
            }
        }
    }
}