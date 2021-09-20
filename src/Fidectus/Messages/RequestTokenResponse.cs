using Shared.Messages;
using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class RequestTokenResponse : ApiResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}