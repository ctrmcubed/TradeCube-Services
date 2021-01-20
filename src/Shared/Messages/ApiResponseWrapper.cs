namespace Shared.Messages
{
    public class ApiResponseWrapper<T> : ApiResponse
    {
        public int? RecordCount { get; set; }
        public T Data { get; set; }

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
