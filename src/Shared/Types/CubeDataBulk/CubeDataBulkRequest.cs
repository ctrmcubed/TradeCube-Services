using System;
using System.Collections.Generic;

namespace Shared.Types.CubeDataBulk;

public class CubeDataBulkRequest
{
    public string Name { get; set; }
    public string Cube { get; set; }
    public string Description { get; set; }
    public string Timezone { get; set; }
    public string DataItem { get; set; }
    public string Node { get; set; }
    public string Layer { get; set; }
    public string Unit { get; set; }
    public bool CreateNodes { get; set; }
    public string Reason { get; set; }
    public int? RegularDayPeriods { get; set; }
    public int? ShortDayPeriods { get; set; }
    public int? LongDayPeriods { get; set; }
    public string ShortDayRule { get; set; }
    public string LongDayRule { get; set; }
    public DateTime? DeleteBeforeUTC { get; set; }
    public string DeleteBeforeLocal { get; set; }
    public DateTime? DeleteAfterUTC { get; set; }
    public string DeleteAfterLocal { get; set; }
    public IEnumerable<CubeDataBulkData> Data { get; set; }
}