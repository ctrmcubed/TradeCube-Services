namespace Fidectus.Models
{
    public class FixedPriceInformation
    {
        public string FixedPricePayer { get; set; }
        public string FPCurrencyUnit { get; set; }
        public string FPCapacityUnit { get; set; }
        public float FPCapacityConversionRate { get; set; }
        public FxInformation FXInformation { get; set; }
    }
}