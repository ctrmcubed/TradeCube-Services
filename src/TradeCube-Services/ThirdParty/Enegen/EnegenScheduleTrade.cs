﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TradeCube_Services.ThirdParty.Enegen
{
    public class EnegenScheduleTrade
    {
        [JsonPropertyName("tradeId")]
        public int TradeId { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("counterparty")]
        public string Counterparty { get; set; }

        [JsonPropertyName("scheduleTradeDetails")]
        public IEnumerable<EnegenScheduleTradeDetail> ScheduleTradeDetails { get; set; }
    }
}