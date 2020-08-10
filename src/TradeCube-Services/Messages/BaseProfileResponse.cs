using Newtonsoft.Json;
using System;

namespace TradeCube_Services.Messages
{
    public class BaseProfileResponse
    {
        [JsonProperty("UTCStartDateTime")]
        public DateTime UtcStartDateTime { get; set; }

        [JsonProperty("UTCEndDateTime")]
        public DateTime UtcEndDateTime { get; set; }

        public decimal Value { get; set; }
    }
}