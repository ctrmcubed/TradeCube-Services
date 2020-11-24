using System.Collections.Generic;

namespace TradeCube_Services.DataObjects
{
    public class PartyDataObject
    {
        public string Party { get; set; }
        public string PartyLongName { get; set; }
        public List<string> PartyType { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Countries { get; set; }

        public bool Internal { get; set; }

        // ReSharper disable once InconsistentNaming
        public EnergyIdentificationCodeDataObject EIC { get; set; }

        // ReSharper disable once InconsistentNaming
        public LegalEntityIdentifierDataObject LEI { get; set; }

        // ReSharper disable once InconsistentNaming
        public BusinessIdentifierCodeDataObject BIC { get; set; }

        // ReSharper disable once InconsistentNaming
        public AcerRegistrationCodeDataObject ACERCode { get; set; }

        public ContactDataObject PrimaryBillingContact { get; set; }

        public ContactDataObject PrimaryConfirmationContact { get; set; }
    }
}