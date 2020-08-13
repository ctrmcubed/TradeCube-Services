using System.Collections.Generic;

namespace TradeCube_Services.Parameters
{
    public class EnegenGenstarTradeParameters : ParametersBase
    {
        public string ActionName { get; set; }
        public string Template { get; set; }
        public string Format { get; set; }
        public IEnumerable<string> TradeReferences { get; set; }
    }
}