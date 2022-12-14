using System.Text.Json.Serialization;

namespace TradeCube_ServicesTests.Enegen;

public class EnegenGenstarHmacResponse
{
    [JsonPropertyName("HMAC")]
    public string Hmac { get; init; }
}