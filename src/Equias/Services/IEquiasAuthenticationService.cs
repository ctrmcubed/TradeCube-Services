using Equias.Messages;
using System.Threading.Tasks;

namespace Equias.Services
{
    public interface IEquiasAuthenticationService
    {
        Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest);
    }
}