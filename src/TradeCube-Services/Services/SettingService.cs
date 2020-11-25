using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TradeCube_Services.Configuration;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public class SettingService : TradeCubeApiService, ISettingService
    {
        public SettingService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<MappingService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingAsync(string setting, string apiKey)
        {
            return await GetViaApiKeyAsync<SettingDataObject>($"SystemSetting/{setting}", apiKey);
        }

        public async Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingsAsync(string apiKey)
        {
            return await GetViaApiKeyAsync<SettingDataObject>("SystemSetting", apiKey);
        }

    }
}