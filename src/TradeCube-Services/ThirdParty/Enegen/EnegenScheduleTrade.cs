using Newtonsoft.Json;
using System.Collections.Generic;

namespace TradeCube_Services.ThirdParty.Enegen
{
    public class EnegenScheduleTrade
    {
        [JsonProperty("tradeId")]
        public int TradeId { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("counterparty")]
        public string Counterparty { get; set; }

        [JsonProperty("scheduleTradeDetails")]
        public IEnumerable<EnegenScheduleTradeDetail> ScheduleTradeDetails { get; set; }
    }
}