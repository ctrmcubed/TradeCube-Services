using System.Collections.Generic;

namespace Shared.Messages;

public class ElexonImbalancePriceResponse : ApiResponse
{
    public IEnumerable<ElexonImbalancePriceItem> Data { get; init; }    
}