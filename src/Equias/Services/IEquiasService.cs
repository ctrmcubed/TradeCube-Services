using Equias.Messages;
using Equias.Models.BackOfficeServices;
using System.Threading.Tasks;

namespace Equias.Services
{
    public interface IEquiasService
    {
        Task<AddPhysicalTradeResponse> AddPhysicalTrade(PhysicalTrade physicalTrade, RequestTokenResponse requestTokenResponse);
    }
}