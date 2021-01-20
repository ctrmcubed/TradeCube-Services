using System.Collections.Generic;

namespace Shared.Messages
{
    public class ProductPriceType
    {
        public string Product { get; set; }
        public IEnumerable<PriceType> Prices { get; set; }
    }
}
