namespace Fidectus.Models
{
    public class UsElectricityTradeDetails
    {
        public string DeliveryType { get; set; }
        public string Type { get; set; }
        public float Voltage { get; set; }
        public ContingencyDetails ContingencyDetails { get; set; }
        public ElectingPartyDetails ElectingPartyDetails { get; set; }
    }
}