using System;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class EquiasType
    {
        [JsonPropertyName("EBOTradeId")]
        public string EboTradeId { get; set; }

        [JsonPropertyName("EBOTradeVersion")]
        public int? EboTradeVersion { get; set; }

        [JsonPropertyName("EBOSubmissionTime")]
        public DateTime? EboSubmissionTime { get; set; }

        [JsonPropertyName("EBOSubmissionStatus")]
        public string EboSubmissionStatus { get; set; }

        [JsonPropertyName("EBOStatusLastCheckedTime")]
        public DateTime? EboStatusLastCheckedTime { get; set; }

        [JsonPropertyName("EBOSubmissionMessage")]
        public string EboSubmissionMessage { get; set; }

        [JsonPropertyName("EBOActionType")]
        public string EboActionType { get; set; }

        [JsonPropertyName("EBOWhithhold")]
        public bool? EboWhithhold { get; set; }

        [JsonPropertyName("CMStatus")]
        public string CmStatus { get; set; }

        [JsonPropertyName("CMMessage")]
        public string CmMessage { get; set; }

        [JsonPropertyName("BFStatus")]
        public string BfStatus { get; set; }
    }
}