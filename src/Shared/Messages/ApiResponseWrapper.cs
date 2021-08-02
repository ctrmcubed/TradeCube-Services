using System.Collections.Generic;

namespace Shared.Messages
{
    public class ApiResponseWrapper<T>
    {
        public int? RecordCount { get; set; }
        public T Data { get; set; }

        public bool IsSuccessStatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string Warning { get; set; }
        public long? ErrorCount { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public ApiResponseWrapper()
        {
        }

        public ApiResponseWrapper(string status, T data)
        {
            Status = status;
            Data = data;
        }
    }
}
