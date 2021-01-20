using System;
using System.Text.Json.Serialization;

namespace Shared.Messages
{
    public class ProfileDefinitionType
    {
        [JsonPropertyName("UTCStartDateTime")]
        public DateTime? UtcStartDateTime;

        [JsonPropertyName("UTCEndDateTime")]
        public DateTime? UtcEndDateTime;
    }
}