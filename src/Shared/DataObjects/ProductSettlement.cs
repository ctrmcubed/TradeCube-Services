using System;

namespace Shared.DataObjects
{
    public class ProductSettlement
    {
        public string SettlementType { get; set; }
        public string SettlementPeriod { get; set; }
        public DateTime? SettlementDate { get; set; }
        public SettlementRuleDataObject BillRule { get; set; }
        public SettlementRuleDataObject PayRule { get; set; }
    }
}