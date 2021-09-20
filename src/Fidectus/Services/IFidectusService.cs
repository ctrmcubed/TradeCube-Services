using Fidectus.Messages;
using Shared.Configuration;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public interface IFidectusService
    {
        Task<ConfirmationResponse> SendConfirmation(string method, string companyId, ConfirmationRequest confirmationRequest, RequestTokenResponse requestTokenResponse,
            IFidectusConfiguration fidectusConfiguration);

        Task<ConfirmationResponse> DeleteConfirmation(string companyId, string docId,
            RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration);

        Task<BoxResultResponse> GetBoxResult(string companyId, string docId, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration);
    }
}