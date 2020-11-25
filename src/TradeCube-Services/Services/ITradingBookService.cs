using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface ITradingBookService
    {
        Task<ApiResponseWrapper<IEnumerable<TradingBookDataObject>>> GetTradingBookAsync(string tradingBook, string apiKey);
        Task<TradingBookDataObject> MapTradingBookAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey);
    }
}