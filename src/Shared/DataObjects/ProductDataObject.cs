using Shared.Messages;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class ProductDataObject
    {
        public string Product { get; set; }
        public string ProductLongName { get; set; }
        public string ProductType { get; set; }
        public CommodityDataObject Commodity { get; set; }
        public string ShapeDescription { get; set; }
        public string PeriodDescription { get; set; }
        public decimal ProductValue { get; set; }

        public string ContractType { get; set; }

        public IEnumerable<ProfileDefinitionType> ProfileDefinition { get; set; }

        public QuantityUnitDataObject QuantityUnit { get; set; }

        public PriceUnitDataObject PriceUnit { get; set; }

        public List<string> PriceUnits { get; set; }

        public List<string> QuantityUnits { get; set; }

        public ProductSettlement Settlement { get; set; }

        public ProductPhysical Physical { get; set; }

        public ProductFinancial Financial { get; set; }

        public ProductOption Option { get; set; }

        public ProductCascade Cascade { get; set; }

        public ProductCashflow Cashflow { get; set; }

        public ProductTradingPeriod TradingPeriod { get; set; }

        public IEnumerable<TimeProfile> ProductProfile { get; set; }

        public List<string> FormulaProducts { get; set; }

        [JsonPropertyName("UPI")]
        public string Upi { get; set; }

        [JsonPropertyName("ISIN")]
        public string Isin { get; set; }

        [JsonPropertyName("CFI")]
        public string Cfi { get; set; }

        public string Image { get; set; }

        public string Fingerprint { get; set; }

        public string PriceFormula { get; set; }
    }
}