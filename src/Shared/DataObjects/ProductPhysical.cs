namespace Shared.DataObjects
{
    public class ProductPhysical
    {
        public string UTCLocal { get; init; }
        public DeliveryAreaDataObject DeliveryArea { get; init; }

        public InterconnectorDataObject Interconnector { get; init; }

        public ResourceDataObject Resource { get; init; }
    }
}