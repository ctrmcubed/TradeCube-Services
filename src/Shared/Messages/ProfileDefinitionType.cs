using System;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Messages
{
    public class ProfileDefinitionType
    {
        [JsonPropertyName("UTCStartDateTime")]
        [BsonElement("UTCStartDateTime")]
        public DateTime? UtcStartDateTime { get; init; }

        [JsonPropertyName("UTCEndDateTime")]
        [BsonElement("UTCEndDateTime")]
        public DateTime? UtcEndDateTime { get; init; }
        
        public string LocalStartDateTime { get; init; }
        public string LocalEndDateTime { get; init; }
        public string ShortDayRule { get; init; }
        public string LongDayRule { get; init; }
        public int[] ApplicableDays { get; init; }
        public int[] ApplicableHours { get; init; }
    }
}