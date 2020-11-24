using System.Text.Json.Serialization;

namespace TradeCube_Services.DataObjects
{
    public class AcerRegistrationCodeDataObject
    {
        [JsonPropertyName("ACERCode")]
        public string AcerCode { get; set; }

        [JsonPropertyName("ACERLongName")]
        public string AcerLongName { get; set; }

        public AddressType Address { get; set; }

        [JsonPropertyName("EIC")]
        public string Eic { get; set; }

        [JsonPropertyName("BIC")]
        public string Bic { get; set; }

        [JsonPropertyName("LEI")]
        public string Lei { get; set; }

        public string Website { get; set; }
        public string TransparencyWebsite { get; set; }
    }
}