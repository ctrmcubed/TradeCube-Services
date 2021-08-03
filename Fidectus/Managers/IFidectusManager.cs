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
        Task<SendConfirmationResponse> ProcessConfirmationAsync(string tradeReference, int tradeLeg, string apiJwtToken, FidectusConfiguration fidectusConfiguration);
        Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<TradeConfirmation> CreateTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken, IFidectusConfiguration fidectusConfiguration);
    }
}