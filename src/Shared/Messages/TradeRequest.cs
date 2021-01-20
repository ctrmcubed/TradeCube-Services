using System.Collections.Generic;

namespace Shared.Messages
{
    public class TradeRequest
    {
        public IEnumerable<string> TradeReferences { get; set; }
    }
}
