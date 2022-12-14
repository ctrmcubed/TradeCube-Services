using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Messages;

namespace Shared.Services;

public class ElexonSettlementPeriodService : TradeCubeApiService, IElexonSettlementPeriodService
{
    private readonly IHttpClientFactory httpClientFactory;

    public ElexonSettlementPeriodService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
        ILogger<ElexonSettlementPeriodService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
    {
        this.httpClientFactory = httpClientFactory;
    }
    
    public async Task<ElexonSettlementPeriodResponse> ElexonSettlementPeriodsAsync(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest, string apiJwtToken)
    {
        return await PostAsJsonAsync<ElexonSettlementPeriodRequest, ElexonSettlementPeriodResponse>(httpClientFactory.CreateClient(), apiJwtToken, elexonSettlementPeriodRequest);
    }
}