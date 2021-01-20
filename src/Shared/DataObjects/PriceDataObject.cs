using System.Collections.Generic;

namespace Shared.DataObjects
{
    public class PriceDataObject
    {
        public string PriceType { get; set; }
        public decimal? Price { get; set; }
        public PriceUnitDataObject PriceUnit { get; set; }
        public IEnumerable<TimeProfile> PriceProfile { get; set; }
    }
}