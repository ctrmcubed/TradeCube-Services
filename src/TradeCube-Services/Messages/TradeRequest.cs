using System.Collections.Generic;

namespace TradeCube_Services.Messages
{
    public class TradeRequest
    {
        public IEnumerable<string> TradeReferences { get; set; }
    }
}
