using System.Collections.Generic;

namespace Shared.DataObjects
{
    public class ContractDataObject
    {
        public string ContractReference { get; set; }
        public string ContractLongName { get; set; }
        public PartyDataObject PrimaryInternalParty { get; set; }
        public PartyDataObject PrimaryCounterparty { get; set; }
        public CommodityDataObject PrimaryCommodity { get; set; }
        public IEnumerable<string> AdditionalInternalParties { get; set; }
        public IEnumerable<string> AdditionalCounterparties { get; set; }
        public IEnumerable<string> AdditionalCommodities { get; set; }
        public AgreementTypeDataObject AgreementType { get; set; }
        public ValidityType Validity { get; set; }
        public ExecutionType Execution { get; set; }
        public string Notes { get; set; }
        public IEnumerable<FileUpload> Attachments { get; set; }
    }
}