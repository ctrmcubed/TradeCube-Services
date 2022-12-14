using System.Text.Json.Serialization;

namespace Shared.Messages;

public class ElexonSettlementPeriodRequest
{
    [JsonPropertyName("UTCStartDateTime")]
    public string UtcStartDateTime { get; init; }
    
    [JsonPropertyName("UTCEndDateTime")]
    public string UtcEndDateTime { get; init; }
}