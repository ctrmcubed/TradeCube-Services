using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public class TradingBookService : TradeCubeApiService, ITradingBookService
    {
        public TradingBookService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<TradingBookDataObject>>> GetTradingBookAsync(string tradingBook, string apiKey)
        {
            return await GetViaApiKeyAsync<TradingBookDataObject>($"TradingBook/{tradingBook}", apiKey);
        }

        public async Task<TradingBookDataObject> MapTradingBookAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey)
        {
            async Task<TradingBookDataObject> Default()
            {
                return allSettings.ContainsKey("M7_DefaultTradingBook")
                    ? (await GetTradingBookAsync(allSettings["M7_DefaultTradingBook"]?.SettingValue, apiKey))?.Data?.SingleOrDefault()
                    : null;
            }

            if (allMappings.ContainsKey("M7_TradingBook"))
            {
                var tradingBook = (await GetTradingBookAsync(allMappings["M7_TradingBook"]?.MappingTo, apiKey))?.Data?.SingleOrDefault();
                return tradingBook ?? await Default();
            }

            return await Default();
        }
    }
}