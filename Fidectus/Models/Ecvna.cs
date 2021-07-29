using System.Text.Json.Serialization;

namespace Fidectus.Models
{
    public class Ecvna
    {
        [JsonPropertyName("BSCPartyID")]
        public string BscPartyId { get; set; }

        public string BuyerEnergyAccount { get; set; }
        public string SellerEnergyAccount { get; set; }

        [JsonPropertyName("BuyerID")]
        public string BuyerId { get; set; }

        [JsonPropertyName("SellerID")]
        public string SellerId { get; set; }
    }
}