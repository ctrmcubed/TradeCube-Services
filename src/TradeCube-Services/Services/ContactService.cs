using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TradeCube_Services.Configuration;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public class ContactService : TradeCubeApiService, IContactService
    {
        public ContactService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<ContactDataObject>>> GetContactAsync(string contact, string apiKey)
        {
            return await GetViaApiKeyAsync<ContactDataObject>($"Contact/{contact}", apiKey);
        }

        public async Task<ContactDataObject> MapInternalTraderAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey)
        {
            async Task<ContactDataObject> Default()
            {
                return allSettings.ContainsKey("M7_DefaultTrader")
                    ? (await GetContactAsync(allSettings["M7_DefaultTrader"]?.SettingValue, apiKey))?.Data?.SingleOrDefault()
                    : null;
            }

            if (allMappings.ContainsKey("M7_InternalTrader"))
            {
                var contact = (await GetContactAsync(allMappings["M7_InternalTrader"]?.MappingTo, apiKey))?.Data?.SingleOrDefault();
                return contact ?? await Default();
            }

            return await Default();
        }
    }
}