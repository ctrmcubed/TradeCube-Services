using System.Collections.Generic;
using Shared.Messages;

public class ElexonSettlementPeriodResponse : ApiResponse
{
    public IEnumerable<ElexonSettlementPeriodResponseItem> Data { get; set; }
}
