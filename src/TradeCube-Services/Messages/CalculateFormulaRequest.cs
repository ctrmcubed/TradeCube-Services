using System.Collections.Generic;

namespace TradeCube_Services.Messages
{
    public class CalculateFormulaRequest
    {
        public int TimeoutSeconds { get; set; }
        public string Formula { get; set; }
        public IEnumerable<FormulaRequestType> Data { get; set; }
    }
}