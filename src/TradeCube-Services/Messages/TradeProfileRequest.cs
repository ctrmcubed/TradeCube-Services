namespace TradeCube_Services.Messages
{
    public class TradeProfileRequest
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }
        public ProfileRequestFormat ProfileFormat { get; set; }
        public bool? Volume { get; set; }
        public bool? Price { get; set; }
    }
}