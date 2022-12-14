using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Messages;

namespace Shared.Services;

public class TradeDetailService : TradeCubeApiService, ITradeDetailService
{
    public TradeDetailService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
        ILogger<TradeDetailService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
    {
    }

    public async Task<ApiResponseWrapper<IEnumerable<TradeDetailResponse>>> GetTradeDetailAsync(string tradeReference, int tradeLeg, string apiJwtToken)
    {
        return await GetViaJwtAsync<TradeDetailResponse>("TradeDetail", apiJwtToken, $"{tradeReference}?TradeLeg={tradeLeg}");
    }
}