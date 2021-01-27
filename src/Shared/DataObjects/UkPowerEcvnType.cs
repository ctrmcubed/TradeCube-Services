using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class UkPowerEcvnType
    {
        [JsonPropertyName("BSCPartyID")]
        public string BscPartyId { get; set; }

        [JsonPropertyName("BuyerEnergyAccount")]
        public string BuyerEnergyAccount { get; set; }

        [JsonPropertyName("SellerEnergyAccount")]
        public string SellerEnergyAccount { get; set; }

        [JsonPropertyName("BuyerID")]
        public string BuyerId { get; set; }

        [JsonPropertyName("SellerID")]
        public string SellerId { get; set; }
    }
}