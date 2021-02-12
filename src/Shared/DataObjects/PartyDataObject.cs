﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class PartyDataObject
    {
        public string Party { get; set; }
        public string PartyLongName { get; set; }
        public List<string> PartyType { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Countries { get; set; }

        public bool Internal { get; set; }

        [JsonPropertyName("EIC")]
        public EnergyIdentificationCodeDataObject Eic { get; set; }

        [JsonPropertyName("LEI")]
        public LegalEntityIdentifierDataObject Lei { get; set; }

        [JsonPropertyName("BIC")]
        public BusinessIdentifierCodeDataObject Bic { get; set; }

        [JsonPropertyName("ACERCode")]
        public AcerRegistrationCodeDataObject AcerCode { get; set; }

        public ContactDataObject PrimaryBillingContact { get; set; }

        public ContactDataObject PrimaryConfirmationContact { get; set; }

        public string Image { get; set; }

        public PartyExtension Extension { get; set; }
    }

    public class UkGasShipperDataObject
    {
        public string ShipperCode { get; set; }
        public string ShipperLongName { get; set; }

        [JsonPropertyName("EIC")]
        public EnergyIdentificationCodeDataObject Eic { get; set; }

        public string State { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}