using Fidectus.Messages;
using Fidectus.Models;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System.Threading.Tasks;

namespace Fidectus.Managers
{
    public interface IFidectusManager
    {
        Task<FidectusConfiguration> GetFidectusConfiguration(string apiJwtToken);
        Task<ConfirmationResponse> ConfirmAsync(TradeKey tradeKey, string apiJwtToken, IFidectusConfiguration fidectusConfiguration);
        Task<ConfirmationResponse> CancelAsync(TradeKey tradeKey, string apiJwtToken, IFidectusConfiguration getFidectusConfiguration);
        Task<ConfirmationResultResponse> BoxResult(TradeKey tradeKey, string apiJwtToken, IFidectusConfiguration fidectusConfiguration);

        Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<TradeConfirmation> CreateTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken, IFidectusConfiguration fidectusConfiguration);
    }
}