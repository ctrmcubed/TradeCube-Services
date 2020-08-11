using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TradeCube_Services.ThirdParty.Enegen
{
    public class EnegenInboundTrade
    {
        [JsonPropertyName("scheduleDates")]
        public IEnumerable<EnegenScheduleDate> ScheduleDates { get; set; }
    }
}
