using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TradeCube_Services.ThirdParty.Enegen
{
    public class EnegenScheduleDate
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("scheduleTrades")]
        public IEnumerable<EnegenScheduleTrade> ScheduleTrades { get; set; }
    }
}