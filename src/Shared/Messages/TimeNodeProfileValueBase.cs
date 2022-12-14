using System;
using System.Text.Json.Serialization;

namespace Shared.Messages;

public class TimeNodeProfileValueBase
{
    public string TimeNode { get; set; }
        
    [JsonPropertyName("UTCStartDateTime")]
    public DateTime UtcStartDateTime { get; set; }
        
    [JsonPropertyName("UTCEndDateTime")]
    public DateTime UtcEndDateTime { get; set; }
        
    public decimal Volume { get; set; }
    public decimal Price { get; set; }
    public decimal Value { get; set; }

    public decimal? MarketPrice { get; set; }
    public decimal? MarkToMarket { get; set; }
    public decimal? ProfitAndLoss { get; set; }
}