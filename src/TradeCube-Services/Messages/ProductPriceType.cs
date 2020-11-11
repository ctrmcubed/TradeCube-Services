using System.Collections.Generic;

namespace TradeCube_Services.Messages
{
    public class ProductPriceType
    {
        public string Product { get; set; }
        public IEnumerable<PriceType> Prices { get; set; }
    }
}
