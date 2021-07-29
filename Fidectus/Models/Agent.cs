using System.Text.Json.Serialization;

namespace Fidectus.Models
{
    public class Agent
    {
        public string AgentType { get; set; }
        public string AgentName { get; set; }
        public Broker Broker { get; set; }

        [JsonPropertyName("ECVNA")]
        public Ecvna Ecvna { get; set; }
    }
}