using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class RequestTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}