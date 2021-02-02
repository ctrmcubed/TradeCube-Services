using Equias.Messages;
using Equias.Models.BackOfficeServices;
using Shared.DataObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equias.Managers
{
    public interface IEquiasManager
    {
        Task<EboGetTradeStatusResponse> TradeStatus(IEnumerable<TradeKey> tradeKeys, string apiJwtToken);
        Task<EboTradeResponse> CreatePhysicalTrade(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<PhysicalTrade> CreatePhysicalTrade(TradeDataObject tradeDataObject, string apiJwtToken);
        Task<EboTradeResponse> AddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, string apiJwtToken);
        Task<EboTradeResponse> CancelTrade(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);

        // For integration tests
        Task<RequestTokenResponse> CreateAuthenticationToken(RequestTokenRequest requestTokenRequest, string apiJwtToken);
    }
}