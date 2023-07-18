using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects
{
    public class PartyExtension
    {
        [JsonPropertyName("BSCParty")]
        [BsonElement("BSCParty")]
        public UkBscPartyDataObject BscParty { get; set; }

        [JsonPropertyName("UKGasShipper")]
        [BsonElement("UKGasShipper")]
        public UkGasShipperDataObject UkGasShipper { get; set; }

        public string DefaultNotificationAgent { get; set; }
        public string DefaultEnergyAccount { get; set; }
        public string DefaultReplacementRule { get; set; }
        public string DefaultSchedule5 { get; set; }

    }
}