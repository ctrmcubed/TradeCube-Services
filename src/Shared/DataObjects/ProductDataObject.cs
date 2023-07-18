using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Shared.Messages;

namespace Shared.DataObjects
{
    public class ProductDataObject
    {
        public string Product { get; init; }

        public string ProductLongName { get; init; }

        public string ProductType { get; init; }

        public string ShapeDescription { get; init; }

        public string PeriodDescription { get; init; }

        public decimal ProductValue { get; init; }

        public string ContractType { get; init; }
        public CommodityDataObject Commodity { get; init; }

        public QuantityUnitDataObject QuantityUnit { get; init; }
        public PriceUnitDataObject PriceUnit { get; init; }
        public IEnumerable<ProfileDefinitionType> ProfileDefinition { get; init; }
    
        public List<string> PriceUnits { get; init; }

        public List<string> QuantityUnits { get; init; }

        public ProductSettlement Settlement { get; init; }

        public ProductPhysical Physical { get; init; }

        public ProductFinancial Financial { get; init; }

        public ProductOption Option { get; init; }

        public ProductCascade Cascade { get; init; }

        public ProductCashflow Cashflow { get; init; }

        public ProductTradingPeriod TradingPeriod { get; init; }

        public IEnumerable<ProfileType> ProductProfile { get; init; }

        public List<string> FormulaProducts { get; init; }

        public string UPI { get; init; }

        public string ISIN { get; init; }

        public string CFI { get; init; }

        public string Image { get; init; }
        public string ImageTopLine { get; init; }
        public string ImageBottomLine { get; init; }

        public string Fingerprint { get; init; }

        public string PriceFormula { get; init; }
        
        [BsonIgnoreIfNull]
        public VisibilityType Visibility { get; init; }  
    }
}