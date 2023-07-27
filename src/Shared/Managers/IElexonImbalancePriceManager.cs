using System.Threading.Tasks;
using Shared.Messages;

namespace Shared.Managers;

public interface IElexonImbalancePriceManager
{
    Task<ElexonImbalancePriceResponse> ElexonImbalancePrice(ElexonImbalancePriceRequest elexonImbalancePriceRequest);
}