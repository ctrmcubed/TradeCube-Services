using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface IVenueService
    {
        Task<ApiResponseWrapper<IEnumerable<VenueDataObject>>> GetVenueAsync(string venue, string apiKey);
        Task<VenueDataObject> MapVenueAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey);
    }
}