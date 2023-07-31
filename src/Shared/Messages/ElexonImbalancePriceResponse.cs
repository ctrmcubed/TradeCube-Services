using System.Collections.Generic;
using Shared.Types.CubeDataBulk;

namespace Shared.Messages;

public class ElexonImbalancePriceResponse : ApiResponse
{
    public IEnumerable<ElexonImbalancePriceItem> Data { get; init; }    
    public CubeDataBulkRequest CubeDataBulk { get; init; }
}