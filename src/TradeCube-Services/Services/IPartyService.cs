using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface IPartyService
    {
        Task<ApiResponseWrapper<IEnumerable<PartyDataObject>>> GetPartyAsync(string party, string apiKey);
    }
}