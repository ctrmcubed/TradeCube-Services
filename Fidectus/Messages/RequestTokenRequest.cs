using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class RequestTokenRequest
    {
        [JsonProperty("client_id")]
        [JsonPropertyName("client_id")]
        public string ClientId { get; }

        [JsonProperty("client_secret")]
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; }

        [JsonProperty("audience")]
        [JsonPropertyName("audience")]
        public string Audience { get; }

        [JsonProperty("grant_type")]
        [JsonPropertyName("grant_type")]
        public string GrantType { get; }

        public RequestTokenRequest(string username, string password)
        {
            ClientId = username;
            ClientSecret = password;
            Audience = "fidectus_open_api_staging";
            GrantType = "client_credentials";
        }
    }
}
