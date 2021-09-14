namespace Fidectus.Models
{
    public class TimeIntervalQuantity
    {
        public string DeliveryStartTimestamp { get; set; }
        public string DeliveryEndTimestamp  { get; set; }
        public decimal? ContractCapacity { get; set; }
        public decimal? Price { get; set; }
    }
}