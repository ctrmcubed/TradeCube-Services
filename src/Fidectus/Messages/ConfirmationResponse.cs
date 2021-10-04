using Shared.Messages;
using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class ConfirmationResponse : ApiResponse
    {
        public string DocumentId { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("message")]
        public new string Message { get; set; }

        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("status")]
        public new int Status { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}
