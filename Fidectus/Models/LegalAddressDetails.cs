namespace Fidectus.Models
{
    public class LegalAddressDetails
    {
        public Street Street { get; set; }
        public StreetNumber StreetNumber { get; set; }
        public City City { get; set; }
        public PostalCode PostalCode { get; set; }
        public State State { get; set; }
        public Country Country { get; set; }
    }
}