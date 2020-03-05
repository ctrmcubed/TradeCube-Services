namespace TradeCube_Services.DataObjects
{
    public class ContactDataObject
    {
        public string Contact { get; set; }
        public string ContactLongName { get; set; }
        public ContactPrimaryAddress PrimaryAddress { get; set; }
    }
}