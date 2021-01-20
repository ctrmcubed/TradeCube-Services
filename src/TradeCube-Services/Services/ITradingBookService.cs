using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public interface ITradingBookService
    {
        Task<ApiResponseWrapper<IEnumerable<TradingBookDataObject>>> GetTradingBookAsync(string tradingBook, string apiKey);
        Task<TradingBookDataObject> MapTradingBookAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey);
    }
}