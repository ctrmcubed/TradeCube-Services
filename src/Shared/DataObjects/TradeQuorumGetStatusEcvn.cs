using System.Text.Json.Serialization;

public class TradeQuorumGetStatusEcvn
{
    [JsonPropertyName("ImportID")]
    public int? ImportId { get; init; }
    
    public string Filename { get; init; }
    public string Status { get; init; }
}