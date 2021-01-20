using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public interface ISettingService
    {
        Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingAsync(string setting, string apiKey);
        Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingsAsync(string apiKey);
    }
}