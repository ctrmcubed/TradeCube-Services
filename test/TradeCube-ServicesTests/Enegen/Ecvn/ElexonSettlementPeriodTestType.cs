using System.Collections.Generic;
using Shared.Messages;

namespace TradeCube_ServicesTests.Enegen.Ecvn;

public class ElexonSettlementPeriodTestType
{
    public ElexonSettlementPeriodRequest Inputs { get; init; }
    public ApiResponseWrapper<IList<ElexonSettlementPeriodResponseItem>> Response { get; init; }  
}