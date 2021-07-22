using Fidectus.Messages;
using Shared.Configuration;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public interface IFidectusAuthenticationService
    {
        Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest, IFidectusConfiguration fidectusConfiguration);
    }
}