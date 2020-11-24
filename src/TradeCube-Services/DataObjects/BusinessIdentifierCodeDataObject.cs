using System.Text.Json.Serialization;

namespace TradeCube_Services.DataObjects
{
    public class BusinessIdentifierCodeDataObject
    {
        [JsonPropertyName("BIC")]
        public string Bic { get; set; }

        public string Bank { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Branch { get; set; }
    }
}