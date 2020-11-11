namespace TradeCube_Services.Messages
{
    public class WebServiceResponse
    {
        public string WebService { get; set; }
        public string ActionName { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
