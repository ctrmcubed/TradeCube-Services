using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Equias.Models.BackOfficeServices
{
    public class PhysicalTrade
    {
        [JsonPropertyName("TradeID")]
        public string TradeId { get; set; }

        [JsonPropertyName("UTI")]
        public string Uti { get; set; }

        public ProcessInformation ProcessInformation { get; set; }
        public UpfrontPaymentInformation UpfrontPaymentInformation { get; set; }
        public string Market { get; set; }
        public string Commodity { get; set; }
        public string TransactionType { get; set; }
        public string DeliveryPointArea { get; set; }
        public string BuyerParty { get; set; }
        public string SellerParty { get; set; }

        [JsonPropertyName("BeneficiaryID")]
        public string BeneficiaryId { get; set; }

        public bool Intragroup { get; set; }
        public string LoadType { get; set; }
        public string MasterAgreementVersion { get; set; }
        public string Agreement { get; set; }
        public decimal TotalVolume { get; set; }
        public string TotalVolumeUnit { get; set; }
        public string TradeExecutionTimestamp { get; set; }
        public string CapacityUnit { get; set; }
        public PriceUnit PriceUnit { get; set; }
        public decimal TotalContractValue { get; set; }
        public string SettlementCurrency { get; set; }
        public IEnumerable<string> SettlementDates { get; set; }
        public IEnumerable<string> LinkedTransactionIdentifiers { get; set; }
        public IEnumerable<TimeIntervalQuantity> TimeIntervalQuantities { get; set; }

        [JsonPropertyName("EUATradeDetails")] 
        public EuaTradeDetails EuaTradeDetails { get; set; }

        public string TradeName { get; set; }
        public HubCodificationInformation HubCodificationInformation { get; set; }
        public OptionDetails OptionDetails { get; set; }
        public IEnumerable<Agent> Agents { get; set; }
    }
}
