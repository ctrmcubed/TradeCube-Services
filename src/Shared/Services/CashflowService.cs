using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class CashflowService : TradeCubeApiService, ICashflowService
    {
        public CashflowService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<CashflowType>>> CashflowAsync(string tradeReference, int tradeLeg, string apiJwtToken)
        {
            return await GetViaJwtAsync<CashflowType>("Cashflow", apiJwtToken, $"{tradeReference}?TradeLeg={tradeLeg}");
        }
    }
}