using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects
{
    public class ExternalFieldsType
    {
        public EquiasType Equias { get; set; }
        public ConfirmationType Confirmation { get; set; }
    
        [JsonPropertyName("ECVN")]
        [BsonElement("ECVN")]
        public TradeEcvnType Ecvn { get; init; }
    
        [JsonPropertyName("QuorumECVN")]
        [BsonElement("QuorumECVN")]
        public QuorumEcvnType QuorumEcvn { get; set; }
    }
}