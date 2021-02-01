using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class SettingService : TradeCubeApiService, ISettingService
    {
        public SettingService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<SettingService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingViaApiKeyAsync(string setting, string apiKey)
        {
            return await GetViaApiKeyAsync<SettingDataObject>($"SystemSetting/{setting}", apiKey);
        }

        public async Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingsViaApiKeyAsync(string apiKey)
        {
            return await GetViaApiKeyAsync<SettingDataObject>("SystemSetting", apiKey);
        }

        public async Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingViaJwtAsync(string setting, string apiJwtToken)
        {
            return await GetViaJwtAsync<SettingDataObject>($"SystemSetting/{setting}", apiJwtToken);
        }

        public async Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingsViaJwtAsync(string apiJwtToken)
        {
            return await GetViaJwtAsync<SettingDataObject>("SystemSetting", apiJwtToken);
        }
    }
}