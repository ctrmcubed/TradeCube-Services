namespace Fidectus.Models
{
    public class PercentageTolerance
    {
        public NegativeLimit NegativeLimit { get; set; }
        public PositiveLimit PositiveLimit { get; set; }
        public ToleranceOptionOwner ToleranceOptionOwner { get; set; }
    }
}