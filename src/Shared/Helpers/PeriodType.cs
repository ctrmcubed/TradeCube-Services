using System;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Shared.Helpers;

[DebuggerDisplay("UTCStart={UtcStart}, DstFlag={DstFlag}")]
public class PeriodType
{
    [JsonPropertyName("UTCStart")]
    public DateTime UtcStart { get; init; }
    
    public DateTime OffsetStart { get; init; }
    public DateTime LocalStart { get; init; }
    
    [JsonPropertyName("DSTFlag")]
    public bool DstFlag { get; init; }
}