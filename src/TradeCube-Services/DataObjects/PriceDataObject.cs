namespace TradeCube_Services.DataObjects
{
    public class PriceDataObject
    {
        public decimal Price { get; set; }
        public PriceUnitDataObject PriceUnit { get; set; }
    }
}