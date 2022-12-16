namespace Shared.Messages
{
    public class ApiResponseWrapper<T> : ApiResponse
    {
        public int? RecordCount { get; init; }
        public T Data { get; init; }

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
