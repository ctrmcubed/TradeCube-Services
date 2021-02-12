using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class UkBscPartyDataObject
    {
        [JsonPropertyName("BSCPartyId")]
        public string BscPartyId { get; set; }

        [JsonPropertyName("BSCPartyLongName")]
        public string BscPartyLongName { get; set; }

        [JsonPropertyName("BSCPartyAddress")]
        public string BscPartyAddress { get; set; }

        [JsonPropertyName("BSCPartyRoles")]
        public List<string> BscPartyRoles { get; set; }
    }
}