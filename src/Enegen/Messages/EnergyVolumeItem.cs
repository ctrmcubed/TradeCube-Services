using System.Text.Json.Serialization;

namespace Enegen.Messages;

public class EnergyVolumeItem
{
    [JsonPropertyName("ECVDate")]
    public string EcvDate { get; init; }
    
    [JsonPropertyName("ECVPeriod")]
    public int EcvPeriod { get; init; }
    
    [JsonPropertyName("ECVVolume")]
    public decimal? EcvVolume { get; init; }
}