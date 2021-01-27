using Equias.Models.BackOfficeServices;
using System.Text.Json.Serialization;

namespace Equias.Messages
{
    public class EboGetTradeStatus
    {
        public string TradeId { get; set; }
        public int? TradeVersion { get; set; }

        [JsonPropertyName("CMTradeStatus")]
        public CmTradeStatus CmTradeStatus { get; set; }

        [JsonPropertyName("BFTradeStatus")]
        public BfTradeStatus BfTradeStatus { get; set; }

        [JsonPropertyName("RRTradeStatus")]
        public RrTradeStatus RrTradeStatus { get; set; }

        [JsonPropertyName("SMTradeStatus")]
        public SmTradeStatus SmTradeStatus { get; set; }
    }
}