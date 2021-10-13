using Shared.Messages;
using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class BoxResultResponse : ApiResponse
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; }

        public BoxResult BoxResult { get; set; }
    }
}