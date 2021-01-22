using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class VenueDataObject
    {
        public string Venue { get; set; }
        public string VenueLongName { get; set; }
        public List<string> VenueType { get; set; }
        public List<string> Countries { get; set; }
        public string PrimaryCountry { get; set; }
        public string PrimaryCurrency { get; set; }
        public CommodityDataObject PrimaryCommodity { get; set; }
        public string Image { get; set; }
        public List<string> Commodities { get; set; }

        [JsonPropertyName("MIC")]
        public string Mic { get; set; }
    }
}