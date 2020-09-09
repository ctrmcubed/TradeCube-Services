using System.Collections.Generic;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Formula
{
    public class FormulaDataWrapper
    {
        public IEnumerable<FormulaRequestType> Data { get; set; }
    }
}