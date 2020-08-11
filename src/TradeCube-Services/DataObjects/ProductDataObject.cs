namespace TradeCube_Services.DataObjects
{
    public class ProductDataObject
    {
        public string Product { get; set; }
        public CommodityDataObject Commodity { get; set; }
        public ProductSettlement Settlement { get; set; }
    }
}