using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Messages;

namespace Shared.DataObjects
{
    public class ProductDataObject
    {
        public string Product { get; set; }

        public string ProductLongName { get; set; }

        public string ProductType { get; set; }

        public string ShapeDescription { get; set; }

        public string PeriodDescription { get; set; }

        public decimal ProductValue { get; set; }

        public string ContractType { get; set; }

        public CommodityDataObject Commodity { get; set; }

        public QuantityUnitDataObject QuantityUnit { get; set; }

        public PriceUnitDataObject PriceUnit { get; set; }
        public IEnumerable<ProfileDefinitionType> ProfileDefinition { get; set; }
    
        public List<string> PriceUnits { get; set; }

        public List<string> QuantityUnits { get; set; }

        public ProductSettlement Settlement { get; set; }

        public ProductPhysical Physical { get; set; }

        public ProductFinancial Financial { get; set; }

        public ProductOption Option { get; set; }

        public ProductCascade Cascade { get; set; }

        public ProductCashflow Cashflow { get; set; }

        public ProductTradingPeriod TradingPeriod { get; set; }

        public IEnumerable<ProfileType> ProductProfile { get; set; }

        public List<string> FormulaProducts { get; set; }

        public string UPI { get; set; }

        public string ISIN { get; set; }

        public string CFI { get; set; }

        public string Image { get; set; }
        public string ImageTopLine { get; set; }
        public string ImageBottomLine { get; set; }

        public string Fingerprint { get; set; }

        public string PriceFormula { get; set; }
        
        [BsonIgnoreIfNull]
        public VisibilityType Visibility { get; set; }  
    }
}