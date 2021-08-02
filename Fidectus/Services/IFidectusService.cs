using Fidectus.Messages;
using Shared.Configuration;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public interface IFidectusService
    {
        Task<TradeConfirmationResponse> FidectusSendTradeConfirmation(TradeConfirmationRequest tradeConfirmationRequest, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration);
    }
}