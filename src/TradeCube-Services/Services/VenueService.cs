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
    public class VenueService : TradeCubeApiService, IVenueService
    {
        public VenueService(IHttpClientFactory httpClientFactory, ITradeCubeConfiguration tradeCubeConfiguration,
            ILogger<TradeCubeApiService> logger) : base(httpClientFactory, tradeCubeConfiguration, logger)
        {
        }

        public async Task<ApiResponseWrapper<IEnumerable<VenueDataObject>>> GetVenueAsync(string venue, string apiKey)
        {
            return await GetViaApiKeyAsync<VenueDataObject>($"Venue/{venue}", apiKey);
        }

        public async Task<VenueDataObject> MapVenueAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey)
        {
            async Task<VenueDataObject> Default()
            {
                return allSettings.ContainsKey("M7_DefaultVenue")
                    ? (await GetVenueAsync(allSettings["M7_DefaultVenue"]?.SettingValue, apiKey))?.Data?.SingleOrDefault()
                    : null;
            }

            if (allMappings.ContainsKey("M7_Venue"))
            {
                var venue = (await GetVenueAsync(allMappings["M7_Venue"]?.MappingTo, apiKey))?.Data?.SingleOrDefault();
                return venue ?? await Default();
            }

            return await Default();
        }
    }
}