using Fidectus.Messages;
using Shared.Configuration;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public interface IFidectusService
    {
        Task<ConfirmationResponse> SendTradeConfirmation(string method, string companyId, TradeConfirmationRequest tradeConfirmationRequest, RequestTokenResponse requestTokenResponse,
            IFidectusConfiguration fidectusConfiguration);

        Task<BoxResultResponse> GetBoxResult(string companyId, string docId, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration);
    }
}