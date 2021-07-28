using System;

namespace Fidectus.Models
{
    public class TimeIntervalQuantity
    {
        public DateTime DeliveryStartDateAndTime { get; set; }
        public DateTime DeliveryEndDateAndTime { get; set; }
        public float ContractCapacity { get; set; }
        public decimal? Price { get; set; }
    }
}