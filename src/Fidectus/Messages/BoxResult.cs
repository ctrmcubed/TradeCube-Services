using System;
using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class BoxResult
    {
        public string DocumentType { get; set; }

        [JsonPropertyName("DocumentID")]
        public string DocumentId { get; set; }

        public int DocumentVersion { get; set; }
        public string State { get; set; }
        public DateTime Timestamp { get; set; }
        public BoxResultCounterparty Counterparty { get; set; }
    }
}