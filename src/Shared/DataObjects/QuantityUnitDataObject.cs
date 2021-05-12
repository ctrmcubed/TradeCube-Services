namespace Shared.DataObjects
{
    public class QuantityUnitDataObject
    {
        public string QuantityUnit { get; set; }
        public string QuantityUnitLongName { get; set; }
        public string Format { get; set; }
        public string Suffix { get; set; }
        public EnergyUnitDataObject EnergyUnit { get; set; }
        public string PerPeriod { get; set; }
    }
}