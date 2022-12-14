using System.Text.Json.Serialization;

namespace TradeCube_ServicesTests.Enegen;

public class EnegenGenstarHmacRequest
{
    [JsonPropertyName("uri")]
    public string Uri { get; init; }
    
    [JsonPropertyName("body")]
    public string Body { get; init; }
    
    [JsonPropertyName("app_id")]
    public string AppId { get; init; }
    
    [JsonPropertyName("private_shared_key")]
    public string PrivateSharedKey { get; init; }
    
    [JsonPropertyName("nonce")]
    public string Nonce { get; init; }
    
    [JsonPropertyName("time_stamp")]
    public int TimeStamp { get; init; }
}