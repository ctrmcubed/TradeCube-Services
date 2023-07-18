using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects
{
    public class EnergyIdentificationCodeDataObject
    {
        [JsonPropertyName("EIC")]
        [BsonElement("EIC")]
        public string Eic { get; init; }

        [JsonPropertyName("EICLongName")]
        [BsonElement("EICLongName")]
        public string EicLongName { get; init; }

        [JsonPropertyName("EICType")]
        [BsonElement("EICType")]
        public string EicType { get; init; }

        public string DisplayName { get; init; }

        [JsonPropertyName("VATCode")]
        [BsonElement("VATCode")]
        public string VatCode { get; init; }

        [JsonPropertyName("EICParent")]
        [BsonElement("EICParent")]
        public string EicParent { get; init; }

        [JsonPropertyName("EICResponsible")]
        [BsonElement("EICResponsible")]
        public string EicResponsible { get; init; }

        public string PostalCode { get; init; }
        public string Country { get; init; }
        public string Function { get; init; }
    }
}