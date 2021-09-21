namespace Shared.Messages
{
    public class TradeKey
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }

        public TradeKey()
        {
        }

        public TradeKey(string tradeReference, int tradeLeg)
        {
            TradeReference = tradeReference;
            TradeLeg = tradeLeg;
        }
    }
}