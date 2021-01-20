using Shared.DataObjects;
using System.Collections.Generic;

namespace Shared.Messages
{
    public class FormulaRequestType
    {
        public TradeDataObject Trade { get; set; }
        public ProfileResponse Profile { get; set; }
        public IEnumerable<ProductPriceType> ProductPrices { get; set; }
    }
}