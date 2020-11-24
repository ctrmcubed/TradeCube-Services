using System.Collections.Generic;
using TradeCube_Services.Messages;

namespace TradeCube_Services.DataObjects
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

        // ReSharper disable once InconsistentNaming
        public string UPI { get; set; }

        // ReSharper disable once InconsistentNaming
        public string ISIN { get; set; }

        // ReSharper disable once InconsistentNaming
        public string CFI { get; set; }

        public string Image { get; set; }

        public string Fingerprint { get; set; }

        public string PriceFormula { get; set; }
    }

    public class ProductPhysical
    {
        public DeliveryAreaDataObject DeliveryArea { get; set; }

        public InterconnectorDataObject Interconnector { get; set; }

        public ResourceDataObject Resource { get; set; }
    }

    public class InterconnectorDataObject
    {
        public string Interconnector { get; set; }
        public string InterconnectorLongName { get; set; }

        // ReSharper disable once InconsistentNaming
        public string EIC { get; set; }
    }
}