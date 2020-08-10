using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TradeCube_Services.ThirdParty.Enegen
{
    public class EnegenScheduleDate
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("scheduleTrades")]
        public IEnumerable<EnegenScheduleTrade> ScheduleTrades { get; set; }
    }
}