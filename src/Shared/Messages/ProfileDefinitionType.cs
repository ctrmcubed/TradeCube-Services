using System;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Messages
{
    public class ProfileDefinitionType
    {
        [JsonPropertyName("UTCStartDateTime")]
        [BsonElement("UTCStartDateTime")]
        public DateTime? UtcStartDateTime { get; set; }

        [JsonPropertyName("UTCEndDateTime")]
        [BsonElement("UTCEndDateTime")]
        public DateTime? UtcEndDateTime { get; set; }
        
        public string LocalStartDateTime { get; set; }
        public string LocalEndDateTime { get; set; }
        public string ShortDayRule { get; set; }
        public string LongDayRule { get; set; }
        public int[] ApplicableDays { get; set; }
        public int[] ApplicableHours { get; set; }
    }
}