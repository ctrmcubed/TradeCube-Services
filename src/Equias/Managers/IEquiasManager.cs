using Equias.Messages;
using Equias.Models.BackOfficeServices;
using Shared.DataObjects;
using Shared.Messages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equias.Managers
{
    public interface IEquiasManager
    {
        Task<EboGetTradeStatusResponse> TradeStatusAsync(IEnumerable<TradeKey> tradeKeys, string apiJwtToken);
        Task<EboTradeResponse> CreatePhysicalTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<PhysicalTrade> CreatePhysicalTradeAsync(TradeDataObject tradeDataObject, string apiJwtToken);
        Task<EboTradeResponse> AddPhysicalTradeAsync(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, string apiJwtToken);
        Task<EboTradeResponse> CancelTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);

        // For integration tests
        Task<RequestTokenResponse> CreateAuthenticationTokenAsync(RequestTokenRequest requestTokenRequest, string apiJwtToken);
    }
}