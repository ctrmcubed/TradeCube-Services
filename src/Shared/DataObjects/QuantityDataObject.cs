namespace Shared.DataObjects
{
    public class QuantityDataObject
    {
        public string QuantityType { get; set; }
        public decimal? Quantity { get; set; }
        public QuantityUnitDataObject QuantityUnit { get; set; }
    }
}