using System.Collections.Generic;

namespace Shared.Messages;

public class ElexonSettlementPeriodResponse : ApiResponse
{
    public IEnumerable<ElexonSettlementPeriodResponseItem> Data { get; set; }
}