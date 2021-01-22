using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IPartyService
    {
        Task<ApiResponseWrapper<IEnumerable<PartyDataObject>>> GetPartyAsync(string party, string apiJwtToken);
    }
}