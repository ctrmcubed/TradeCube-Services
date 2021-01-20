using System.Collections.Generic;

namespace Shared.Messages
{
    public class FingerprintRequest
    {
        public string Commodity { get; set; }
        public IEnumerable<ProfileDefinitionType> ProfileDefinition { get; set; }
    }
}
