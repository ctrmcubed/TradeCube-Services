using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Messages;

namespace Shared.Services;

public class ElexonSettlementPeriodService : TradeCubeApiService, IElexonSettlementPeriodService
{
    public ElexonSettlementPeriodService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
        ILogger<ElexonSettlementPeriodService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
    {
    }
    
    public async Task<ApiResponseWrapper<IEnumerable<ElexonSettlementPeriodResponseItem>>> ElexonSettlementPeriodsAsync(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest, string apiJwtToken)
    {
        return await TradeCubePostViaJwtAsync<ElexonSettlementPeriodRequest, ApiResponseWrapper<IEnumerable<ElexonSettlementPeriodResponseItem>>>(apiJwtToken, "ElexonSettlementPeriod", elexonSettlementPeriodRequest);
    }
}