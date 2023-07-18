using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects
{
    public class UkBscPartyDataObject
    {
        [JsonPropertyName("BSCPartyId")]
        [BsonElement("BSCPartyId")]
        public string BscPartyId { get; init; }

        [JsonPropertyName("BSCPartyLongName")]
        [BsonElement("BSCPartyLongName")]
        public string BscPartyLongName { get; init; }

        [JsonPropertyName("BSCPartyAddress")]
        [BsonElement("BSCPartyAddress")]
        public string BscPartyAddress { get; init; }

        [JsonPropertyName("BSCPartyRoles")]
        [BsonElement("BSCPartyRoles")]
        public List<string> BscPartyRoles { get; init; }
    }
}