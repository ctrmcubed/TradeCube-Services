using Newtonsoft.Json;

namespace TradeCube_Services.ThirdParty.Enegen
{
    public class EnegenScheduleTradeDetail
    {
        [JsonProperty("settlementPeriod")]
        public int SettlementPeriod { get; set; }

        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}