namespace Fidectus.Models
{
    public class DeliveryPeriod
    {
        public string DeliveryPeriodEndDate { get; set; }
        public float DeliveryPeriodNotionalQuantity { get; set; }
        public string DeliveryPeriodStartDate { get; set; }
        public string PaymentDate { get; set; }
        public float FixedPrice { get; set; }
    }
}