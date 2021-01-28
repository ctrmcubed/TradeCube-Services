using System.Collections.Generic;

namespace Shared.DataObjects
{
    public class ContactDataObject
    {
        public string Contact { get; set; }
        public string ContactLongName { get; set; }
        public PartyDataObject Party { get; set; }
        public string ContactType { get; set; }
        public string PrimaryEmailAddress { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public IEnumerable<string> AlternateEmailAddresses { get; set; }
        public IEnumerable<string> AlternatePhoneNumbers { get; set; }
        public ContactPrimaryAddress PrimaryAddress { get; set; }
        public ContactAlternateAddress AlternateAddress { get; set; }
    }
}