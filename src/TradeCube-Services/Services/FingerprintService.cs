using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Messages;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public class FingerprintService : TradeCubeApiService, IFingerprintService
    {
        private readonly ILogger<TradeCubeApiService> logger;

        public FingerprintService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<FingerprintService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<IEnumerable<FingerprintResponse>>> FingerprintAsync(string apiKey, FingerprintRequest fingerprintRequest)
        {
            try
            {
                return await TradeCubePostViaApiKeyAsync<FingerprintRequest, ApiResponseWrapper<IEnumerable<FingerprintResponse>>>(apiKey, "Fingerprint", fingerprintRequest);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return new ApiResponseWrapper<IEnumerable<FingerprintResponse>>
                {
                    Message = ex.Message,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Data = new List<FingerprintResponse>()
                };
            }
        }
    }
}