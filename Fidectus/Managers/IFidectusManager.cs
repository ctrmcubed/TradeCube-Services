using Fidectus.Helpers;
using Fidectus.Messages;
using Fidectus.Models;
using Shared.Configuration;
using Shared.DataObjects;
using System.Threading.Tasks;

namespace Fidectus.Managers
{
    public interface IFidectusManager
    {
        Task<FidectusConfiguration> GetFidectusConfiguration(string apiJwtToken);
        Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<(TradeConfirmation tradeConfirmation, ConfigurationHelper configurationHelper)> CreateTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken);
        Task<GetConfirmationResponse> GetConfirmationAsync(string tradeId, RequestTokenResponse requestTokenResponse, FidectusConfiguration fidectusConfiguration);
        Task<SendConfirmationResponse> SendConfirmationAsync(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<SendConfirmationResponse> SendConfirmationAsync(string tradeReference, int tradeLeg, string apiJwtToken, FidectusConfiguration fidectusConfiguration);
        Task<SendConfirmationResponse> SendConfirmationAsync(TradeConfirmation tradeConfirmation, string apiJwtToken);
    }
}