using System;

namespace Shared.DataObjects
{
    public class ConfirmationType
    {
        public string DocumentId { get; set; }
        public int Version { get; set; }
        public DateTime SubmissionTime { get; set; }
        public DateTime LastCheckedTime { get; set; }

        public string SubmissionStatus { get; set; }
        public string Message { get; set; }
        public bool Withhold { get; set; }

        public string CMStatus { get; set; }
        public string CMMessage { get; set; }

        public string CounterpartyDocumentId { get; set; }
        public string CounterpartyVersion { get; set; }
    }
}