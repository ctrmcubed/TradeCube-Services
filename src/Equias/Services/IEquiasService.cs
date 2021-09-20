using Equias.Messages;
using Equias.Models.BackOfficeServices;
using Shared.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Equias.Services
{
    public interface IEquiasService
    {
        Task<EboGetTradeStatusResponse> EboGetTradeStatus(IEnumerable<string> tradeIds, RequestTokenResponse requestTokenResponse, IEquiasConfiguration equiasConfiguration);
        Task<EboTradeResponse> EboAddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, IEquiasConfiguration equiasConfiguration);
        Task<EboTradeResponse> ModifyPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse, IEquiasConfiguration equiasConfiguration);
        Task<EboTradeResponse> CancelTrade(CancelTrade cancelTrade, RequestTokenResponse requestTokenResponse, IEquiasConfiguration equiasConfiguration);

    }
}