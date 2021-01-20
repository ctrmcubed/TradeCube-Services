using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class EnergyIdentificationCodeDataObject
    {
        [JsonPropertyName("EIC")]
        public string Eic { get; set; }

        [JsonPropertyName("EICLongName")]
        public string EicLongName { get; set; }

        [JsonPropertyName("EICType")]
        public string EicType { get; set; }

        public string DisplayName { get; set; }

        [JsonPropertyName("VATCode")]
        public string VatCode { get; set; }

        [JsonPropertyName("EICParent")]
        public string EicParent { get; set; }

        [JsonPropertyName("EICResponsible")]
        public string EicResponsible { get; set; }

        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Function { get; set; }
    }
}