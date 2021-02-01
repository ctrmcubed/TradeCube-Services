using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface ISettingService
    {
        Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingViaApiKeyAsync(string setting, string apiKey);
        Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingsViaApiKeyAsync(string apiKey);
        Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingViaJwtAsync(string setting, string apiJwtToken);
        Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingsViaJwtAsync(string apiJwtToken);
    }
}