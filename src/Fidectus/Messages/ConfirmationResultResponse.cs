using Shared.Messages;
using System;

namespace Fidectus.Messages
{
    public class ConfirmationResultResponse : ApiResponse
    {
        public string Id { get; set; }
        public string DocumentId { get; set; }
        public int? DocumentVersion { get; set; }
        public string DocumentType { get; set; }
        public string State { get; set; }
        public DateTime? Timestamp { get; set; }
        public string CounterpartyDocumentId { get; set; }
        public int? CounterpartyDocumentVersion { get; set; }
    }
}