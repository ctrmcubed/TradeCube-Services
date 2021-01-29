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
        Task<RequestTokenRequest> GetAuthenticationToken(string apiJwtToken);
        Task<RequestTokenResponse> GetAuthenticationToken(RequestTokenRequest requestTokenRequest);
        Task<EboGetTradeStatusResponse> EboGetTradeStatus(IEnumerable<string> tradeIds, RequestTokenResponse requestTokenResponse);
        Task<TradeDataObject> GetTradeAsync(string tradeReference, int tradeLeg, string apiJwtToken);
        Task<PhysicalTrade> CreatePhysicalTrade(TradeDataObject tradeDataObject, string apiJwtToken);
        Task<EboPhysicalTradeResponse> AddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse);
        Task<EboPhysicalTradeResponse> ModifyPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse);
        Task<ApiResponseWrapper<IEnumerable<TradeDataObject>>> SaveTrade(TradeDataObject tradeDataObject, string apiJwtToken);
        TradeDataObject SetTradePreSubmission(EboGetTradeStatusResponse eboGetTradeStatusResponse, TradeDataObject tradeDataObject);
        TradeDataObject SetTradePostSubmission(EboPhysicalTradeResponse eboAddPhysicalTradeResponse, TradeDataObject tradeDataObject);
    }
}