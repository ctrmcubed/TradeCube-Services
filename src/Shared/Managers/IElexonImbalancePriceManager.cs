using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Messages;
using Shared.Types.Elexon;

namespace Shared.Managers;

public interface IElexonImbalancePriceManager
{
    ElexonImbalancePriceContext CreateContext(ElexonImbalancePriceRequest elexonImbalancePriceRequest);
    ElexonImbalancePriceResponse ElexonImbalancePrice(ElexonImbalancePriceContext elexonImbalancePriceRequest,
        DerivedSystemWideData response, IEnumerable<ElexonSettlementPeriodResponseItem> apiResponseWrapper);
    Task<DerivedSystemWideData> GetElexonDerivedSystemWideData(ElexonImbalancePriceContext elexonImbalancePriceContext);
    ElexonSettlementPeriodResponse GetElexonSettlementPeriods(ElexonImbalancePriceContext elexonImbalancePriceContext);
}