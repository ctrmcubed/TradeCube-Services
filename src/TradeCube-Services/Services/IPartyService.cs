using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public interface IPartyService
    {
        Task<ApiResponseWrapper<IEnumerable<PartyDataObject>>> GetPartyAsync(string party, string apiKey);
        Task<PartyDataObject> MapInternalPartyAsync(string accountId, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey);
        Task<PartyDataObject> MapCounterpartyAsync(string exchangeId, Dictionary<string, MappingDataObject> allMappings, Dictionary<string, SettingDataObject> allSettings, string apiKey);
    }
}