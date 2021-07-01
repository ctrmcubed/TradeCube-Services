namespace Shared.DataObjects
{
    public class ClonedFrom
    {
        public string TradeReference { get; set; }
        public int? TradeLeg { get; set; }
        public string CloneMethod { get; set; }
    }
}