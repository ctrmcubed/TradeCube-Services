using System.Text.Json.Serialization;

namespace Equias.Models.BackOfficeServices
{
    public class Broker
    {
        [JsonPropertyName("BrokerID")] 
        public string BrokerId { get; set; }

        public decimal TotalFee { get; set; }

        public string FeeCurrency { get; set; }
    }
}