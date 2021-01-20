namespace Shared.DataObjects
{
    public class BaseCommodityDataObject
    {
        public string BaseCommodity { get; set; }
        public string BaseCommodityLongName { get; set; }
        public CategoryDataObject Category { get; set; }
    }
}