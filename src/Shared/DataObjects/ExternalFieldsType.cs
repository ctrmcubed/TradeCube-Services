using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class ExternalFieldsType
    {
        [JsonPropertyName("UKGasHub")]
        public UkGasHubType UkGasHub { get; set; }

        [JsonPropertyName("UKPowerECVN")]

        public UkPowerEcvnType UkPowerEcvn { get; set; }
    }
}