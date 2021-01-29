using System;

namespace Shared.DataObjects
{
    public class FeeData
    {
        public string FeeDescription { get; set; }
        public decimal Fee { get; set; }
        public string Currency { get; set; }
        public string FeeType { get; set; }
        public DateTime FeeDateTime { get; set; }
        public PartyDataObject FeePayableToParty { get; set; }
    }
}