using Equias.Messages;
using System.Threading.Tasks;
using Shared.Configuration;

namespace Equias.Services
{
    public interface IEquiasAuthenticationService
    {
        Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest, EquiasConfiguration equiasConfiguration);
    }
}