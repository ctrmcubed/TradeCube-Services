using Equias.Messages;
using Shared.Configuration;
using System.Threading.Tasks;

namespace Equias.Services
{
    public interface IEquiasAuthenticationService
    {
        Task<RequestTokenResponse> EboGetAuthenticationToken(RequestTokenRequest requestTokenRequest, IEquiasConfiguration equiasConfiguration);
    }
}