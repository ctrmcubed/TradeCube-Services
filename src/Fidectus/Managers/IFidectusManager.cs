using Fidectus.Messages;
using Fidectus.Models;
using Shared.Configuration;
using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fidectus.Managers
{
    public interface IFidectusManager
    {
        Task<FidectusConfiguration> GetFidectusConfiguration(string apiJwtToken);
        Task<ConfirmationResponse> ConfirmAsync(string tradeReference, int tradeLeg, string apiJwtToken, IFidectusConfiguration fidectusConfiguration);
        Task<ConfirmationResponse> CancelAsync(string tradeReference, int tradeLeg, string apiJwtToken, IFidectusConfiguration getFidectusConfiguration);
        IAsyncEnumerable<ConfirmationResultResponse> BoxResults(IEnumerable<TradeKey> tradeKeys, string apiJwtToken, IFidectusConfiguration fidectusConfiguration);
        
        Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<TradeConfirmation> CreateTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken, IFidectusConfiguration fidectusConfiguration);
    }
}