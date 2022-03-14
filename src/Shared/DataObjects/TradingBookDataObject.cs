using System.Collections.Generic;

namespace Shared.DataObjects
{
    public class TradingBookDataObject
    {
        public string TradingBook { get; set; }
        public string TradingBookLongName { get; set; }
        public List<string> Categories { get; set; }
        public List<string> BaseCommodities { get; set; }
        public List<string> Commodities { get; set; }
        public ScafellDataObjectMetadata Metadata { get; set; }
        public VisibilityType Visibility { get; set; }
    }
}