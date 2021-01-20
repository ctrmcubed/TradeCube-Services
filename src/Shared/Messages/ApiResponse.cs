using System.Collections.Generic;

namespace Shared.Messages
{
    public class ApiResponse
    {
        public string Status { get; set; }

        public string Message { get; set; }
        public string Warning { get; set; }
        public long? ErrorCount { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
