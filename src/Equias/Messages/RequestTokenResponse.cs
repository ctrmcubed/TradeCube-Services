namespace Equias.Messages
{
    public class RequestTokenResponse
    {
        public string Token { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}