namespace Fidectus.Models
{
    public class PhysicalCoalTradeDetails
    {
        public string Incoterms { get; set; }
        public string Origin { get; set; }
        public string RSS { get; set; }
        public float Tolerance { get; set; }
        public UsCoalProduct USCoalProduct { get; set; }
    }
}