using System;
using System.Text.Json.Serialization;

namespace Shared.Messages
{
    public class ProfileDefinitionType
    {
        [JsonPropertyName("StartDateTimeUTC")]
        public DateTime? StartDateTimeUtc { get; set; }

        [JsonPropertyName("EndDateTimeUTC")]
        public DateTime? EndDateTimeUtc { get; set; }

        public string StartDateTimeLocal { get; set; }
        public string EndDateTimeLocal { get; set; }
        public string ShortDayRule { get; set; }
        public string LongDayRule { get; set; }
        public int[] ApplicableDays { get; set; }
        public int[] ApplicableHours { get; set; }
    }
}