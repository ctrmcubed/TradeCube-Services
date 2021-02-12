using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class PartyExtension
    {
        [JsonPropertyName("BSCParty")]
        public UkBscPartyDataObject BscParty { get; set; }

        [JsonPropertyName("UKGasShipper")]
        public UkGasShipperDataObject UkGasShipper { get; set; }

        public string DefaultNotificationAgent { get; set; }
        public string DefaultEnergyAccount { get; set; }
        public string DefaultReplacementRule { get; set; }
        public string DefaultSchedule5 { get; set; }

    }
}