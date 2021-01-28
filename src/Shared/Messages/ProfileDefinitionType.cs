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

        public string LocalStartDateTime;
        public string LocalEndDateTime;
        public string ShortDayRule { get; set; }
        public string LongDayRule { get; set; }
        public int[] ApplicableDays { get; set; }
        public int[] ApplicableHours { get; set; }
    }
}