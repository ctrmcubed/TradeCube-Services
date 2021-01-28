using System.Collections.Generic;

namespace Shared.DataObjects
{
    public class TradingBookDataObject
    {
        public string TradingBook { get; set; }
        public string TradingBookLongName { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> BaseCommodities { get; set; }
        public IEnumerable<string> Commodities { get; set; }
    }
}