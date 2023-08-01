using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;

namespace Shared.Services;

public class DataItemService: TradeCubeApiService, IDataItemService
{
    public DataItemService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
        ILogger<DataItemService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
    {
    }
        
    public async Task<ApiResponseWrapper<IEnumerable<DataItemDataObject>>> GetDataItem(string dataItem, string jwtApiToken)
    {
        return await GetViaJwtAsync<DataItemDataObject>($"DataItem/{dataItem}", jwtApiToken);
    }
    
    public async Task<ApiResponseWrapper<IEnumerable<DataItemDataObject>>> GetDataItemViaApiKey(string dataItem, string apiKey)
    {
        return await GetViaApiKeyAsync<DataItemDataObject>($"DataItem/{dataItem}", apiKey);
    }
}