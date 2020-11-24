using Newtonsoft.Json;
using System;

namespace TradeCube_Services.DataObjects
{
    public class TimeProfile
    {
        public string LocalStartDateTime { get; set; }
        public string LocalEndDateTime { get; set; }

        [JsonProperty("UTCStartDateTime")]
        public DateTime? UtcStartDateTime { get; set; }

        [JsonProperty("UTCEndDateTime")]
        public DateTime? UtcEndDateTime { get; set; }

        public decimal? Value { get; set; }
    }
}