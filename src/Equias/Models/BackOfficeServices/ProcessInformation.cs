using System.Text.Json.Serialization;

namespace Equias.Models.BackOfficeServices
{
    public class ProcessInformation
    {
        public bool ReportingOnBehalfOf { get; set; }

        [JsonPropertyName("EMIRReportMode")]
        public string EmirReportMode { get; set; }

        [JsonPropertyName("REMITReportMode")]
        public string RemitReportMode { get; set; }
    }
}