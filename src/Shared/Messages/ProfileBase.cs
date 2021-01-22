using System;
using System.Text.Json.Serialization;

namespace Shared.Messages
{
    public class ProfileBase
    {
        [JsonPropertyName("UTCStartDateTime")]
        public DateTime UtcStartDateTime { get; set; }

        [JsonPropertyName("UTCEndDateTime")]
        public DateTime UtcEndDateTime { get; set; }

        public decimal Value { get; set; }
    }
}