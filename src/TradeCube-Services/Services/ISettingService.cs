using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface ISettingService
    {
        Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingAsync(string setting, string apiKey);
        Task<ApiResponseWrapper<IEnumerable<SettingDataObject>>> GetSettingsAsync(string apiKey);
    }
}