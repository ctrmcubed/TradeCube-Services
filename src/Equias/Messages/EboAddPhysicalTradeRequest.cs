namespace Equias.Messages
{
    public class EboAddPhysicalTradeRequest
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }
        public string ActionType { get; set; }
    }
}