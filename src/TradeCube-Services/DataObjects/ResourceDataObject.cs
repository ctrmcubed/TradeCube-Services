using System.Text.Json.Serialization;

namespace TradeCube_Services.DataObjects
{
    public class ResourceDataObject
    {
        public string Resource { get; set; }
        public string ResourceLongName { get; set; }
        public string ResourceType { get; set; }
        public BaseCommodityDataObject BaseCommodity { get; set; }
        public string Country { get; set; }
        public CapacityType Capacity { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string PrimaryFuel { get; set; }
        public string PrimaryOwner { get; set; }

        [JsonPropertyName("EIC")]
        public string Eic { get; set; }
    }
}