using System.Text.Json.Serialization;

namespace Shared.Messages;

public class EnegenGenstarHmacResponse
{
    [JsonPropertyName("HMAC")]
    public string Hmac { get; init; }
}