using System.Text.Json.Serialization;

namespace Equias.Models.BackOfficeServices
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

        public string NotificationAgent { get; set; }
        public string TransmissionChargeIdentification { get; set; }
    }
}