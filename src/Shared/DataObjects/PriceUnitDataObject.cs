namespace Shared.DataObjects
{
    public class PriceUnitDataObject
    {
        public string PriceUnit { get; set; }
        public string PriceUnitLongName { get; set; }
        public string Suffix { get; set; }

        public int? CurrencyExponent { get; set; }
        public string PriceUnitType { get; set; }
        public string Currency { get; set; }
        public string Cryptocurrency { get; set; }

        public int? PerCurrencyExponent { get; set; }
        public string PerPriceUnitType { get; set; }
        public string PerCurrency { get; set; }
        public string PerCryptocurrency { get; set; }
        public string Period { get; set; }
        public QuantityUnitDataObject PerQuantityUnit { get; set; }
    }
}