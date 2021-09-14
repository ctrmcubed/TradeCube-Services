namespace Fidectus.Models
{
    public class FloatPriceInformation
    {
        public string FloatPricePayer { get; set; }
        public string FormulaID { get; set; }
        public FormulaSpreadInformation FormulaSpreadInformation { get; set; }
        public CommodityReference[] CommodityReferences { get; set; }
    }
}