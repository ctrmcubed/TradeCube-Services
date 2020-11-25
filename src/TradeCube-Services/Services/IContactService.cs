using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface IContactService
    {
        Task<ApiResponseWrapper<IEnumerable<ContactDataObject>>> GetContactAsync(string contact, string apiKey);
        Task<ContactDataObject> MapInternalTraderAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey);
    }
}