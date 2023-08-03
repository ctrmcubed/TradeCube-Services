using System;
using System.Collections.Generic;

namespace Shared.Types.CubeDataBulk;

public class CubeDataBulkRequest
{
    public string Name { get; init; }
    public string Cube { get; init; }
    public string Description { get; init; }
    public string Timezone { get; init; }
    public string DataItem { get; init; }
    public string Node { get; init; }
    public string Layer { get; init; }
    public string Unit { get; init; }
    public bool CreateNodes { get; init; }
    public string Reason { get; init; }
    public int? RegularDayPeriods { get; init; }
    public int? ShortDayPeriods { get; init; }
    public int? LongDayPeriods { get; init; }
    public string ShortDayRule { get; init; }
    public string LongDayRule { get; init; }
    public DateTime? DeleteBeforeUTC { get; init; }
    public string DeleteBeforeLocal { get; init; }
    public DateTime? DeleteAfterUTC { get; init; }
    public string DeleteAfterLocal { get; init; }
    public IEnumerable<CubeDataBulkData> Data { get; init; }
}