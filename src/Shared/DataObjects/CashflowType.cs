using System;

namespace Shared.DataObjects
{
    public class CashflowType
    {
        public PartyDataObject CashflowFrom { get; set; }
        public PartyDataObject CashflowTo { get; set; }
        public string CashflowCurrency { get; set; }
        public DateTime BillingDate { get; set; }
        public DateTime SettlementDate { get; set; }
        public decimal SettlementValue { get; set; }
    }
}