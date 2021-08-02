namespace Equias.Messages
{
    public class EboTradeResponse
    {
        public string TradeId { get; set; }
        public int TradeVersion { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public string Message { get; set; }
    }
}