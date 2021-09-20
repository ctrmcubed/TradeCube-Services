using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class BoxResultCounterparty
    {
        [JsonPropertyName("DocumentID")]
        public string DocumentId { get; set; }

        public int DocumentVersion { get; set; }
    }
}