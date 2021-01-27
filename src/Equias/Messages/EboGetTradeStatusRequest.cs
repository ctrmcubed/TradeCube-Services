using System.Collections.Generic;

namespace Equias.Messages
{
    public class EboGetTradeStatusRequest
    {
        public IEnumerable<string> TradeIds { get; set; }
    }
}