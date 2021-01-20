using System;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class LegalEntityIdentifierDataObject
    {
        [JsonPropertyName("LEI")]
        public string Lei { get; set; }

        public string LegalLongName { get; set; }
        public AddressType LegalAddress { get; set; }
        public AddressType HeadquartersAddress { get; set; }

        [JsonPropertyName("RegistrationAuthorityID")]
        public string RegistrationAuthorityId { get; set; }

        [JsonPropertyName("OtherRegistrationAuthorityID")]
        public string OtherRegistrationAuthorityId { get; set; }

        [JsonPropertyName("RegistrationAuthorityEntityID")]
        public string RegistrationAuthorityEntityId { get; set; }

        public string LegalJurisdiction { get; set; }
        public string EntityCategory { get; set; }
        public string EntityLegalFormCode { get; set; }
        public string OtherLegalForm { get; set; }

        [JsonPropertyName("AssociatedLEI")]
        public string AssociatedLei { get; set; }

        public string AssociatedEntityName { get; set; }
        public string EntityStatus { get; set; }

        public DateTime EntityExpirationDate { get; set; }

        public string EntityExpirationReason { get; set; }

        [JsonPropertyName("SuccessorLEI")]
        public string SuccessorLei { get; set; }

        public string SuccessorEntityName { get; set; }
        public DateTime InitialRegistrationDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public string RegistrationStatus { get; set; }

        public DateTime NextRenewalDate { get; set; }

        [JsonPropertyName("ManagingLOU")]
        public string ManagingLou { get; set; }

        public string ValidationSources { get; set; }

        [JsonPropertyName("ValidationAuthorityID")]
        public string ValidationAuthorityId { get; set; }

        [JsonPropertyName("OtherValidationAuthorityID")]
        public string OtherValidationAuthorityId { get; set; }

        [JsonPropertyName("ValidationAuthorityEntityID")]
        public string ValidationAuthorityEntityId { get; set; }
    }
}