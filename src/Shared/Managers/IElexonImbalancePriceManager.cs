using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Messages;
using Shared.Types.Elexon;

namespace Shared.Managers;

public interface IElexonImbalancePriceManager
{
    DerivedSystemWideDataRequest CreateElexonImbalancePriceRequest(ElexonImbalancePriceContext elexonImbalancePriceContext);
    ElexonSettlementPeriodRequest CreateElexonSettlementPeriodRequest(ElexonImbalancePriceContext elexonImbalancePriceContext);

    Task<ElexonImbalancePriceContext> CreateContext(ElexonImbalancePriceRequest elexonImbalancePriceRequest);
    Task<ElexonImbalancePriceResponse> ElexonImbalancePriceWithCdb(ElexonImbalancePriceRequest elexonImbalancePriceRequest);

    ElexonImbalancePriceResponse ElexonImbalancePrice(ElexonImbalancePriceContext elexonImbalancePriceRequest,
        DerivedSystemWideData response, IEnumerable<ElexonSettlementPeriodResponseItem> apiResponseWrapper);

    Task<DerivedSystemWideData> GetElexonDerivedSystemWideData(DerivedSystemWideDataRequest derivedSystemWideDataRequest);

    ElexonSettlementPeriodResponse GetElexonSettlementPeriods(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest);
}