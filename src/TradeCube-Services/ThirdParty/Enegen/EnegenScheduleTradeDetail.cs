using System.Text.Json.Serialization;

namespace TradeCube_Services.ThirdParty.Enegen
{
    public class EnegenScheduleTradeDetail
    {
        [JsonPropertyName("settlementPeriodStartTime")]
        public string SettlementPeriodStartTime { get; set; }

        [JsonPropertyName("volume")]
        public decimal Volume { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}