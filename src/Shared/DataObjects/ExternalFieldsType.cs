using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class ExternalFieldsType
    {
        public EquiasType Equias { get; set; }
        public ConfirmationType Confirmation { get; set; }
        
        [JsonPropertyName("ECVN")]
        public TradeEcvnType Ecvn { get; init; }
        
        [JsonPropertyName("QuorumECVN")]
        public QuorumEcvnType QuorumEcvn { get; set; }
    }
}