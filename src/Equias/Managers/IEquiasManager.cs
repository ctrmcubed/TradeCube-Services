using Equias.Messages;
using Equias.Models.BackOfficeServices;
using System.Threading.Tasks;

namespace Equias.Managers
{
    public interface IEquiasManager
    {
        Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest);
        Task<PhysicalTrade> CreatePhysicalTrade(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<AddPhysicalTradeResponse> AddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse);
    }
}