using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class DeliveryAreaDataObject
    {
        public string DeliveryArea { get; set; }
        public string DeliveryAreaLongName { get; set; }
        public string Country { get; set; }

        [JsonPropertyName("EIC")]
        public string Eic { get; set; }
    }
}