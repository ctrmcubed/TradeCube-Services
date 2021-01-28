using System;

namespace Shared.DataObjects
{
    public class CreditData
    {
        public string CreditDescription { get; set; }
        public decimal Credit { get; set; }
        public string Currency { get; set; }
        public string CreditType { get; set; }
        public DateTime CreditDateTime { get; set; }
        public PartyDataObject CreditPayableFromParty { get; set; }
    }
}