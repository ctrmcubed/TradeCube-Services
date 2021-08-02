using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class RequestTokenRequest
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; }

        [JsonPropertyName("audience")]
        public string Audience { get; }

        [JsonPropertyName("grant_type")]
        public string GrantType { get; }

        public RequestTokenRequest(string username, string password, string audience)
        {
            ClientId = username;
            ClientSecret = password;
            Audience = audience;
            GrantType = "client_credentials";
        }
    }
}
