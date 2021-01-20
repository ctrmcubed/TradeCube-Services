namespace Shared.Messages
{
    public class ApiRequest<T>
    {
        public T Data { get; set; }

        public ApiRequest(T data)
        {
            Data = data;
        }
    }
}