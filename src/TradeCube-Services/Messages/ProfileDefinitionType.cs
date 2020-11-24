using Newtonsoft.Json;
using System;

namespace TradeCube_Services.Messages
{
    public class ProfileDefinitionType
    {
        [JsonProperty("UTCStartDateTime")]
        public DateTime? UtcStartDateTime;

        [JsonProperty("UTCEndDateTime")]
        public DateTime? UtcEndDateTime;
    }
}