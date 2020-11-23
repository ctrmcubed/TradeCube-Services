using System.Collections.Generic;

namespace TradeCube_Services.DataObjects
{
    public class ContractDataObject
    {
        public string ContractReference { get; set; }
        public string ContractLongName { get; set; }
        public PartyDataObject PrimaryInternalParty { get; set; }
        public PartyDataObject PrimaryCounterparty { get; set; }
        public List<string> AdditionalInternalParties { get; set; }
        public List<string> AdditionalCounterparties { get; set; }
        public List<string> AdditionalCommodities { get; set; }
    }
}