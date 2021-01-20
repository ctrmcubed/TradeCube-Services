using Equias.Messages;
using System.Threading.Tasks;

namespace Equias.Managers
{
    public interface IEquiasManager
    {
        Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest);
        Task<AddPhysicalTradeResponse> AddPhysicalTrade(RequestTokenResponse requestTokenResponse);
    }
}