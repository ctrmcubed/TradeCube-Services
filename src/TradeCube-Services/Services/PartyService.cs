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
    public class PartyService : TradeCubeApiService, IPartyService
    {
        public PartyService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<PartyDataObject>>> GetPartyAsync(string party, string apiKey)
        {
            return await GetViaApiKeyAsync<PartyDataObject>($"Party/{party}", apiKey);
        }

        public async Task<PartyDataObject> MapInternalPartyAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey)
        {
            async Task<PartyDataObject> Default()
            {
                return allSettings.ContainsKey("M7_DefaultInternalParty")
                    ? (await GetPartyAsync(allSettings["M7_DefaultInternalParty"]?.SettingValue, apiKey))?.Data?.SingleOrDefault()
                    : null;
            }

            if (allMappings.ContainsKey("M7_InternalParty"))
            {
                var party = (await GetPartyAsync(allMappings["M7_InternalParty"]?.MappingTo, apiKey))?.Data?.SingleOrDefault();
                return party ?? await Default();
            }

            return await Default();
        }

        public async Task<PartyDataObject> MapCounterpartyAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey)
        {
            async Task<PartyDataObject> Default()
            {
                return allSettings.ContainsKey("M7_DefaultParty")
                    ? (await GetPartyAsync(allSettings["M7_DefaultParty"]?.SettingValue, apiKey))?.Data?.SingleOrDefault()
                    : null;
            }

            if (allMappings.ContainsKey("M7_Party"))
            {
                var party = (await GetPartyAsync(allMappings["M7_Party"]?.MappingTo, apiKey))?.Data?.SingleOrDefault();
                return party ?? await Default();
            }

            return await Default();
        }
    }
}