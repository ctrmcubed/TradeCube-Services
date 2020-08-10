using Newtonsoft.Json;
using System.Collections.Generic;

namespace TradeCube_Services.ThirdParty.Enegen
{
    public class EnegenInboundTrade
    {
        [JsonProperty("scheduleDates")]
        public IEnumerable<EnegenScheduleDate> ScheduleDates { get; set; }
    }
}
