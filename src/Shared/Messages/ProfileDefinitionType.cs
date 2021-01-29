using System;
using System.Text.Json.Serialization;

namespace Shared.Messages
{
    public class ProfileDefinitionType
    {
        [JsonPropertyName("UTCStartDateTime")]
        public DateTime? UtcStartDateTime { get; set; }

        [JsonPropertyName("UTCEndDateTime")]
<<<<<<< Updated upstream
        public DateTime? UtcEndDateTime;
=======
        public DateTime? UtcEndDateTime { get; set; }

        public string LocalStartDateTime { get; set; }
        public string LocalEndDateTime { get; set; }
        public string ShortDayRule { get; set; }
        public string LongDayRule { get; set; }
        public int[] ApplicableDays { get; set; }
        public int[] ApplicableHours { get; set; }
>>>>>>> Stashed changes
    }
}