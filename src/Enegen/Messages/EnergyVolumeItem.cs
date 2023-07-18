using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Enegen.Messages;

public class EnergyVolumeItem
{
    [JsonPropertyName("ECVDate")]
    [BsonElement("ECVDate")]
    public string EcvDate { get; init; }
    
    [JsonPropertyName("ECVPeriod")]
    [BsonElement("ECVPeriod")]
    public int EcvPeriod { get; init; }
    
    [JsonPropertyName("ECVVolume")]
    [BsonElement("ECVVolume")]
    public decimal? EcvVolume { get; init; }
}