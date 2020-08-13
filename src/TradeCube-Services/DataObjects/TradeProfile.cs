using System;

namespace TradeCube_Services.DataObjects
{
    public class TradeProfile
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }
        public DateTime Utc { get; set; }

        public DateTime Local { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }
        public int PeriodNumber { get; set; }
    }
}
