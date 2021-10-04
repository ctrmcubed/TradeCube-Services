using System;

namespace Shared.DataObjects
{
    public class ConfirmationType
    {
        public string DocumentId { get; set; }
        public int? DocumentVersion { get; set; }
        public string SubmissionStatus { get; set; }
        public DateTime? SubmissionTime { get; set; }
        public DateTime? LastCheckedTime { get; set; }
        public string SubmissionMessage { get; set; }
        public bool? Withhold { get; set; }
        public bool? Cancel { get; set; }
        public string CMStatus { get; set; }
        public string CMMessage { get; set; }
        public string CounterpartyDocumentId { get; set; }
        public int? CounterpartyDocumentVersion { get; set; }
        public string ConfirmationProvider { get; set; }
    }
}