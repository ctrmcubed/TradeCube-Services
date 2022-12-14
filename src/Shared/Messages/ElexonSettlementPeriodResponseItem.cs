using System.Text.Json.Serialization;

namespace Shared.Messages;

public class ElexonSettlementPeriodResponseItem
{
    [JsonPropertyName("UTCStartDateTime")]
    public string UtcStartDateTime { get; init; }
    
    public string SettlementDate { get; init; }
    
    public int SettlementPeriod { get; init; }
}