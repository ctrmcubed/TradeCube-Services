﻿using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Messages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class ProfileService : TradeCubeApiService, IProfileService
    {
        public ProfileService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<ProfileService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<ProfileResponse>>> GetProfileAsync(string tradeReference, int tradeLeg, string apiJwtToken, string format)
        {
            return await GetViaJwtAsync<ProfileResponse>("Profile", apiJwtToken, $"{tradeReference}?TradeLeg={tradeLeg}&ProfileFormat={format}&Volume=true&Price=True");
        }
    }
}