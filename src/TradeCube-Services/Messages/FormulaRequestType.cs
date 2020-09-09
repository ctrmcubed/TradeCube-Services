using System.Collections.Generic;
using TradeCube_Services.DataObjects;

namespace TradeCube_Services.Messages
{
    public class FormulaRequestType
    {
        public TradeDataObject Trade { get; set; }
        public ProfileResponse Profile { get; set; }
        public IEnumerable<ProductPriceType> ProductPrices { get; set; }
    }
}