namespace Fidectus.Models
{
    public class Agent
    {
        public string AgentType { get; set; }
        public string AgentName { get; set; }
        public Broker Broker { get; set; }
        public Ecvna ECVNA { get; set; }
    }
}