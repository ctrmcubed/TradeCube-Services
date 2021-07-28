using Fidectus.Messages;
using Fidectus.Models;
using Shared.DataObjects;
using System.Threading.Tasks;

namespace Fidectus.Managers
{
    public interface IFidectusManager
    {
        Task<TradeConfirmation> CreateTradeConfirmationAsync(TradeDataObject tradeDataObject, string apiJwtToken);
        Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);

        // For integration tests
        Task<RequestTokenResponse> CreateAuthenticationTokenAsync(RequestTokenRequest requestTokenRequest, string apiJwtToken);
    }
}