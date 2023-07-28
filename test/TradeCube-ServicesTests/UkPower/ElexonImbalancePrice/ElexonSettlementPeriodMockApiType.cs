using System.Collections.Generic;
using Shared.Messages;

namespace TradeCube_ServicesTests.UkPower.ElexonImbalancePrice;

public class ElexonSettlementPeriodMockApiType
{
    public ElexonSettlementPeriodRequest Inputs { get; init; }
    public ApiResponseWrapper<IEnumerable<ElexonSettlementPeriodResponseItem>> Response { get; init; }
}