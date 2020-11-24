using System.Text.Json.Serialization;

namespace TradeCube_Services.DataObjects
{
    public class InterconnectorDataObject
    {
        public string Interconnector { get; set; }
        public string InterconnectorLongName { get; set; }

        [JsonPropertyName("ACERCode")]
        public string EIC { get; set; }
    }
}