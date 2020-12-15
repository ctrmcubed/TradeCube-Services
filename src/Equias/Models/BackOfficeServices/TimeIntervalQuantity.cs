using System;

namespace Equias.Models.BackOfficeServices
{
    public class TimeIntervalQuantity
    {
        public DateTime DeliveryStartTimestamp { get; set; }
        public DateTime DeliveryEndTimestamp { get; set; }
        public decimal Price { get; set; }
        public decimal ContractCapacity { get; set; }
    }
}