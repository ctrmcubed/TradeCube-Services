using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class RequestTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        public bool IsSuccessStatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}