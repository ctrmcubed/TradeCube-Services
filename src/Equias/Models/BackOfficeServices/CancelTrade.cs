using System.Text.Json.Serialization;

namespace Equias.Models.BackOfficeServices
{
    public class CancelTrade
    {
        [JsonPropertyName("TradeID")]
        public string TradeId { get; set; }
    }
}