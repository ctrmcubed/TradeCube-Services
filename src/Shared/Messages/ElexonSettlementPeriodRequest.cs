using System.Text.Json.Serialization;

namespace Shared.Messages;

public class ElexonSettlementPeriodRequest
{
    [JsonPropertyName("StartDateTimeUTC")]
    public string StartDateTimeUtc { get; init; }
    
    [JsonPropertyName("EndDateTimeUTC")]
    public string EndDateTimeUtc { get; init; }
}
