using System.Collections.Generic;

namespace TradeCube_Services.Messages
{
    public class FingerprintRequest
    {
        public string Commodity { get; set; }
        public IEnumerable<ProfileDefinitionType> ProfileDefinition { get; set; }
    }
}
