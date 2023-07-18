using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects
{
    public class PartyDataObject
    {
        public string Party { get; init; }
        public string PartyLongName { get; init; }
        public List<string> PartyType { get; init; }
        public List<string> Categories { get; init; }
        public List<string> Countries { get; init; }
        public bool Internal { get; init; }

        [JsonPropertyName("EIC")]
        [BsonElement("EIC")]
        public EnergyIdentificationCodeDataObject Eic { get; init; }

        [JsonPropertyName("LEI")]
        [BsonElement("LEI")]
        public LegalEntityIdentifierDataObject Lei { get; init; }

        [JsonPropertyName("BIC")]
        [BsonElement("BIC")]
        public BusinessIdentifierCodeDataObject Bic { get; init; }

        [JsonPropertyName("ACERCode")]
        [BsonElement("ACERCode")]
        public AcerRegistrationCodeDataObject AcerCode { get; init; }

        public ContactDataObject PrimaryBillingContact { get; init; }

        public ContactDataObject PrimaryConfirmationContact { get; init; }

        public string Image { get; init; }

        public PartyExtension Extension { get; init; }
        public VisibilityType Visibility { get; init; }
    }
}