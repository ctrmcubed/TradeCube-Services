using System.Text.Json.Serialization;

namespace Shared.Messages;

public class ElexonImbalancePriceItem
{
    public string SettlementDate { get; init; }
    public int SettlementPeriod { get; init; }
    public decimal ImbalancePrice { get; init; }
     
    [JsonPropertyName("StartDateTimeUTC")]
    public string StartDateTimeUtc { get; init; } 
}