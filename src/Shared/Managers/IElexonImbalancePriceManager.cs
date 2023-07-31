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
    DerivedSystemWideData DeserializeDerivedSystemWideData(string response);

    ElexonImbalancePriceResponse ElexonImbalancePrice(ElexonImbalancePriceContext elexonImbalancePriceRequest,
        DerivedSystemWideData response, IEnumerable<ElexonSettlementPeriodResponseItem> apiResponseWrapper);
}