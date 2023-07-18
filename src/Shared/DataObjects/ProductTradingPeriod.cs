using System;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects
{
    public class ProductTradingPeriod
    {
        [JsonPropertyName("UTCStartDateTime")]
        [BsonElement("UTCStartDateTime")]
        public DateTime? UtcStartDateTime { get; init; }
        
        [JsonPropertyName("UTCEndDateTime")]
        [BsonElement("UTCEndDateTime")]
        public DateTime? UtcEndDateTime { get; init; }
    }
}