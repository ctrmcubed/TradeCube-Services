using Fidectus.Messages;
using System.Threading.Tasks;

namespace Fidectus.Managers
{
    public interface IFidectusManager
    {
        // For integration tests
        Task<RequestTokenResponse> CreateAuthenticationTokenAsync(RequestTokenRequest requestTokenRequest, string apiJwtToken);
    }
}