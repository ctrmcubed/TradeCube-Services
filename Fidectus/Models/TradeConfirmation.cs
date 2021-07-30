using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fidectus.Models
{
    public class TradeConfirmation
    {
        public string Agreement { get; set; }
        public string BuyerParty { get; set; }

        [JsonPropertyName("DocumentID")]
        public string DocumentId { get; set; }

        public string DocumentUsage { get; set; }
        public int DocumentVersion { get; set; }

        [JsonPropertyName("ReceiverID")]
        public string ReceiverId { get; set; }

        public string ReceiverRole { get; set; }
        public string SellerParty { get; set; }

        [JsonPropertyName("SenderID")]
        public string SenderId { get; set; }

        public string TransactionType { get; set; }
        public string Market { get; set; }
        public string Commodity { get; set; }
        public string DeliveryPointArea { get; set; }
        public string LoadType { get; set; }
        public Currency Currency { get; set; }
        public decimal? TotalVolume { get; set; }
        public string TotalVolumeUnit { get; set; }
        public string TradeDate { get; set; }
        public string CapacityUnit { get; set; }
        public PriceUnit PriceUnit { get; set; }
        public IEnumerable<TimeIntervalQuantity> TimeIntervalQuantities { get; set; }
        public decimal? TotalContractValue { get; set; }
        public HubCodificationInformation HubCodificationInformation { get; set; }
        public AccountAndChargeInformation AccountAndChargeInformation { get; set; }
        public IEnumerable<Agent> Agents { get; set; }
        public TradeTime TradeTime { get; set; }
        public string TraderName { get; set; }
    }
}