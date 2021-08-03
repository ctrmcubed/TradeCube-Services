using Fidectus.Messages;
using Shared.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public interface IFidectusService
    {
        Task<GetConfirmationResponse> GetTradeConfirmation(string companyId, IEnumerable<string> docIds, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration);
        Task<SendConfirmationResponse> SendTradeConfirmation(string method, string companyId, TradeConfirmationRequest tradeConfirmationRequest, RequestTokenResponse requestTokenResponse, IFidectusConfiguration fidectusConfiguration);
    }
}