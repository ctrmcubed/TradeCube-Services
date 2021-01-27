using Shared.DataObjects;
using System.Collections.Generic;

namespace Shared.Messages
{
    public class CashflowResponse
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }
        public IEnumerable<CashflowType> Cashflows { get; set; }
    }
}
