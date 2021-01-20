using System;

namespace Shared.DataObjects
{
    public class CashflowProfile
    {
        public DateTime LocalStartDateTime { get; set; }
        public DateTime LocalEndDateTime { get; set; }
        public DateTime BillingDate { get; set; }
        public DateTime SettlementDate { get; set; }
        public decimal Multiplier { get; set; }
    }
}