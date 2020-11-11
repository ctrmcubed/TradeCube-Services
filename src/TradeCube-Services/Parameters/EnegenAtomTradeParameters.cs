using System.Collections.Generic;

namespace TradeCube_Services.Parameters
{
    public class EnegenAtomTradeParameters : ParametersBase
    {
        public string Url { get; set; }
        public string WebService { get; set; }
        public IEnumerable<string> TradeReferences { get; set; }
    }
}