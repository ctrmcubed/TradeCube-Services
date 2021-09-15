namespace Shared.Messages
{
    public class SubmitConfirmationRequest
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }
    }
}
