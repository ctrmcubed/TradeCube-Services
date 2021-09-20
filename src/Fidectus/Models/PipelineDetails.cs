namespace Fidectus.Models
{
    public class PipelineDetails
    {
        public DeliverableByBarge DeliverableByBarge { get; set; }
        public EntryPoint EntryPoint { get; set; }
        public IncoTerms IncoTerms { get; set; }
        public PipelineName PipelineName { get; set; }
        public PipelineCycles PipelineCycles { get; set; }
    }
}