using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fidectus.Messages
{
    public class GetConfirmationResponse
    {
        [JsonPropertyName("ECMEnvelopes")]
        public IEnumerable<EcmEnvelope> EcmEnvelopes { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}