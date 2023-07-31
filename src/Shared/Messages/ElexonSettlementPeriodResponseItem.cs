using System.Text.Json.Serialization;

namespace Shared.Messages;

public class ElexonSettlementPeriodResponseItem
{
    [JsonPropertyName("StartDateTimeUTC")]
    public string StartDateTimeUtc { get; init; }
    
    public string SettlementDate { get; init; }
    public int SettlementPeriod { get; init; }
}