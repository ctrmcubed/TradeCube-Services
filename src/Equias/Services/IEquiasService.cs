using Equias.Messages;
using Equias.Models.BackOfficeServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equias.Services
{
    public interface IEquiasService
    {
        Task<EboGetTradeStatusResponse> EboGetTradeStatus(IEnumerable<string> tradeIds, RequestTokenResponse requestTokenResponse);
        Task<EboPhysicalTradeResponse> EboAddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse);
        Task<EboPhysicalTradeResponse> ModifyPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse);
    }
}