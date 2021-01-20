namespace Shared.DataObjects
{
    public class EnergyUnitDataObject
    {
        public string EnergyUnit { get; set; }
        public string EnergyUnitLongName { get; set; }
        public string Format { get; set; }
        public string Suffix { get; set; }
        public decimal? JouleEquivalent { get; set; }
    }
}