using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Messages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class TradeSummaryService : TradeCubeApiService, ITradeSummaryService
    {
        public TradeSummaryService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradeSummaryResponse>>> TradeSummaryAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            return await GetViaJwtAsync<TradeSummaryResponse>("TradeSummary", apiJwtToken, $"{tradeReference}?TradeLeg={tradeLeg}");
        }
    }
}