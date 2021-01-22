using System;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class TimeProfile
    {
        public string LocalStartDateTime { get; set; }
        public string LocalEndDateTime { get; set; }

        [JsonPropertyName("UTCStartDateTime")]
        public DateTime? UtcStartDateTime { get; set; }

        [JsonPropertyName("UTCEndDateTime")]
        public DateTime? UtcEndDateTime { get; set; }

        public decimal? Value { get; set; }
    }
}