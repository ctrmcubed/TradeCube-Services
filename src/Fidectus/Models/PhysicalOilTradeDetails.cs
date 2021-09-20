namespace Fidectus.Models
{
    public class PhysicalOilTradeDetails
    {
        public string Grade { get; set; }
        public string ImporterOfRecord { get; set; }
        public string Incoterms { get; set; }
        public string Type { get; set; }
        public AbsoluteTolerance AbsoluteTolerance { get; set; }
        public PercentageTolerance PercentageTolerance { get; set; }
        public PipelineDetails PipelineDetails { get; set; }
    }
}