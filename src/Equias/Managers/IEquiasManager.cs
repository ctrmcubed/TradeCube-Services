using Equias.Messages;
using Equias.Models.BackOfficeServices;
using Shared.DataObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equias.Managers
{
    public interface IEquiasManager
    {
        Task<RequestTokenRequest> CreateAuthenticationTokenRequest(string apiJwtToken);
        Task<RequestTokenResponse> CreateAuthenticationToken(RequestTokenRequest requestTokenRequest, string apiJwtToken);
        Task<EboGetTradeStatusResponse> EboGetTradeStatus(IEnumerable<TradeKey> tradeKeys, RequestTokenResponse requestTokenResponse, string apiJwtToken);
        Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<EboPhysicalTradeResponse> CreatePhysicalTrade(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<PhysicalTrade> CreatePhysicalTrade(TradeDataObject tradeDataObject, string apiJwtToken);
        Task<EboPhysicalTradeResponse> AddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, string apiJwtToken);
        Task<EboPhysicalTradeResponse> ModifyPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, string apiJwtToken);
    }
}