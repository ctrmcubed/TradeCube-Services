using System.Text.Json.Serialization;

namespace Enegen.Models;

public class EnergyContractedVolumeItem
{
    [JsonPropertyName("ECVDate")]
    public DateTime EcvDate  { get; init; }
    
    [JsonPropertyName("ECVPeriod")]
    public int EcvPeriod  { get; init; }
    
    [JsonPropertyName("ECVVolume")]
    public decimal EcvVolume  { get; init; }
}