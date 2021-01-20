using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public interface IContactService
    {
        Task<ApiResponseWrapper<IEnumerable<ContactDataObject>>> GetContactAsync(string contact, string apiKey);
        Task<ContactDataObject> MapInternalTraderAsync(string key, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey);
    }
}