using Equias.Messages;
using Shared.Configuration;
using System.Threading.Tasks;

namespace Equias.Services
{
    public interface IEquiasAuthenticationService
    {
        Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest, IEquiasConfiguration equiasConfiguration);
    }
}