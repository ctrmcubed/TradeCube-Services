using System;
using System.Text.Json.Serialization;

namespace TradeCube_Services.Messages
{
    public class BaseProfileResponse
    {
        [JsonPropertyName("UTCStartDateTime")]
        public DateTime UtcStartDateTime { get; set; }

        [JsonPropertyName("UTCEndDateTime")]
        public DateTime UtcEndDateTime { get; set; }

        public decimal Value { get; set; }
    }
}