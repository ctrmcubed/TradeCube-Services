using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services;

public class CubeService : TradeCubeApiService, ICubeService
{
    public CubeService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
        ILogger<CubeService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
    {
    }
        
    public async Task<ApiResponseWrapper<IEnumerable<CubeDataObject>>> GetCube(string cubeKey, string jwtApiToken)
    {
        return (await GetViaJwtAsync<CubeDataObject>($"Cube/{cubeKey}", jwtApiToken));
    }
}