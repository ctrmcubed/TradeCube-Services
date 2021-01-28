using System.Collections.Generic;

namespace Shared.DataObjects
{
    public class CommodityDataObject
    {
        public string Commodity { get; set; }
        public string CommodityLongName { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public string Period { get; set; }
        public int? PeriodMinutes { get; set; }
        public DeliveryAreaDataObject DeliveryArea { get; set; }
        public string Timezone { get; set; }
        public EnergyUnitDataObject EnergyUnit { get; set; }
        public BaseCommodityDataObject BaseCommodity { get; set; }
        public QuantityUnitDataObject DefaultQuantityUnit { get; set; }
        public PriceUnitDataObject DefaultPriceUnit { get; set; }
        public List<string> RegulatoryMechanisms { get; set; }
        public SettlementRuleDataObject DefaultBillRule { get; set; }
        public SettlementRuleDataObject DefaultPayRule { get; set; }
        public string Image { get; set; }
    }
}