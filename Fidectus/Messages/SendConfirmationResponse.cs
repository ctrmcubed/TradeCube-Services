using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class SendConfirmationResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        public bool IsSuccessStatusCode { get; set; }
    }
}
