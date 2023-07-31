using System;
using System.Collections.Generic;

namespace Shared.Types.CubeDataBulk;

public class CubeDataBulkData
{
    public DateTime? StartDateTimeUTC { get; set; }
    
    public string StartDateTimeLocal { get; set; }
    public string DataItem { get; set; }
    public string Node { get; set; }
    public string Layer { get; set; }
    public string Unit { get; set; }
    public IEnumerable<decimal> Values { get; set; }
}