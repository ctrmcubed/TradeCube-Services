using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fidectus.Models
{
    public class TradeConfirmation
    {
        public string Agreement { get; set; }
        public string BuyerParty { get; set; }

        [JsonProperty("DocumentID")]
        [JsonPropertyName("DocumentID")]
        public string DocumentId { get; set; }

        public string DocumentUsage { get; set; }
        public int DocumentVersion { get; set; }

        [JsonProperty("ReceiverID")]
        [JsonPropertyName("ReceiverID")]
        public string ReceiverId { get; set; }

        public string ReceiverRole { get; set; }
        public string SellerParty { get; set; }

        [JsonProperty("SenderID")]
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
        public string TotalAmountCurrency { get; set; }
        public string TradeDate { get; set; }
        public DateTime TradeExecutionTimestamp { get; set; }
        public string CapacityUnit { get; set; }
        public PriceUnit PriceUnit { get; set; }
        public IEnumerable<TimeIntervalQuantity> TimeIntervalQuantities { get; set; }
        public FixedPriceInformation FixedPriceInformation { get; set; }
        public decimal? TotalContractValue { get; set; }
        public IEnumerable<FloatPriceInformation> FloatPriceInformation { get; set; }
        public string Rounding { get; set; }
        public bool CommonPricing { get; set; }
        public string OrderNumber { get; set; }
        public string EffectiveDate { get; set; }
        public string TerminationDate { get; set; }
        public EuaTradeDetails EUATradeDetails { get; set; }
        public PhysicalCoalTradeDetails PhysicalCoalTradeDetails { get; set; }
        public PhysicalOilTradeDetails PhysicalOilTradeDetails { get; set; }
        public UsElectricityTradeDetails USElectricityTradeDetails { get; set; }
        public PhysicalBullionTradeDetails PhysicalBullionTradeDetails { get; set; }
        public PhysicalMetalTradeDetails PhysicalMetalTradeDetails { get; set; }
        public HubCodificationInformation HubCodificationInformation { get; set; }
        public AccountAndChargeInformation AccountAndChargeInformation { get; set; }
        public OptionDetails OptionDetails { get; set; }
        public IEnumerable<DeliveryPeriod> DeliveryPeriods { get; set; }
        public IEnumerable<Agent> Agents { get; set; }
        public TradeTime TradeTime { get; set; }
        public string TraderName { get; set; }
        public SchemaDescription SchemaDescription { get; set; }
        public SellerPartyDetails SellerPartyDetails { get; set; }
        public BuyerPartyDetails BuyerPartyDetails { get; set; }
    }
}