using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Types.CubeDataBulk;

namespace Shared.Services;

public class CubeDataBulkService : TradeCubeApiService, ICubeDataBulkService
{
    public CubeDataBulkService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
        ILogger<CubeDataBulkService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
    {
    }

    public async Task<CubeDataBulkResponse> CubeDataBulk(CubeDataBulkRequest cubeDataBulkRequest, string apiKey)
    {
        return await TradeCubePostViaApiKeyAsync<CubeDataBulkRequest, CubeDataBulkResponse>(apiKey, $"CubeDataBulk", cubeDataBulkRequest);
    }
}