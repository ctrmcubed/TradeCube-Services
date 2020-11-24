using System;

namespace TradeCube_Services.DataObjects
{
    public class LegalEntityIdentifierDataObject
    {
        // ReSharper disable once InconsistentNaming
        public string LEI { get; set; }

        public string LegalLongName { get; set; }
        public AddressType LegalAddress { get; set; }
        public AddressType HeadquartersAddress { get; set; }

        // ReSharper disable once InconsistentNaming
        public string RegistrationAuthorityID { get; set; }

        // ReSharper disable once InconsistentNaming
        public string OtherRegistrationAuthorityID { get; set; }

        // ReSharper disable once InconsistentNaming
        public string RegistrationAuthorityEntityID { get; set; }

        public string LegalJurisdiction { get; set; }
        public string EntityCategory { get; set; }
        public string EntityLegalFormCode { get; set; }
        public string OtherLegalForm { get; set; }

        // ReSharper disable once InconsistentNaming
        public string AssociatedLEI { get; set; }

        public string AssociatedEntityName { get; set; }
        public string EntityStatus { get; set; }

        public DateTime EntityExpirationDate { get; set; }

        public string EntityExpirationReason { get; set; }

        // ReSharper disable once InconsistentNaming
        public string SuccessorLEI { get; set; }

        public string SuccessorEntityName { get; set; }
        public DateTime InitialRegistrationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public string RegistrationStatus { get; set; }

        public DateTime NextRenewalDate { get; set; }

        // ReSharper disable once InconsistentNaming
        public string ManagingLOU { get; set; }

        public string ValidationSources { get; set; }

        // ReSharper disable once InconsistentNaming
        public string ValidationAuthorityID { get; set; }

        // ReSharper disable once InconsistentNaming
        public string OtherValidationAuthorityID { get; set; }

        // ReSharper disable once InconsistentNaming
        public string ValidationAuthorityEntityID { get; set; }
    }
}