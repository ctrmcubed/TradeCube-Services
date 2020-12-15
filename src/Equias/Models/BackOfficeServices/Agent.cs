using System.Text.Json.Serialization;

namespace Equias.Models.BackOfficeServices
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