using System;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class ProductTradingPeriod
    {
        [JsonPropertyName("UTCStartDateTime")]
        public DateTime? UtcStartDateTime { get; set; }

        [JsonPropertyName("UTCEndDateTime")]
        public DateTime? UtcEndDateTime { get; set; }
    }
}