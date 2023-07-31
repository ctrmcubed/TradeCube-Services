using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services;

public class CubeTypeService : TradeCubeApiService, ICubeTypeService
{
    public CubeTypeService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
        ILogger<CubeTypeService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
    {
    }

    public async Task<ApiResponseWrapper<IEnumerable<CubeTypeDataObject>>> GetCubeTypes(string jwtApiToken)
    {
        return await GetViaJwtAsync<CubeTypeDataObject>($"CubeType", jwtApiToken);
    }

    public async Task<ApiResponseWrapper<IEnumerable<CubeTypeDataObject>>> GetCubeType(string cubeType, string jwtApiToken)
    {
        return await GetViaJwtAsync<CubeTypeDataObject>($"CubeType/{cubeType}", jwtApiToken);
    }
}