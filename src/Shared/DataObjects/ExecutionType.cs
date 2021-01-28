using System;

namespace Shared.DataObjects
{
    public class ExecutionType
    {
        public DateTime? ExecutedDateTime { get; set; }
        public DateTime? ExpirationDateTime { get; set; }
        public string ExpirationReason { get; set; }
    }
}