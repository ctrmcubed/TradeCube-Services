namespace Fidectus.Models
{
    public class TimeIntervalQuantity
    {
        public string DeliveryStartDateAndTime { get; set; }
        public string DeliveryEndDateAndTime { get; set; }
        public decimal? ContractCapacity { get; set; }
        public decimal? Price { get; set; }
    }
}