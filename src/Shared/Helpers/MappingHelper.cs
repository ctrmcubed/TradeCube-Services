using Shared.DataObjects;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Helpers
{
    public class MappingHelper
    {
        private readonly Dictionary<(string MappingKey, string MappingFrom), MappingDataObject> mappings;

        public MappingHelper(IEnumerable<MappingDataObject> mappings)
        {
            this.mappings = mappings.ToDictionary(k => (k.MappingKey, k.MappingFrom), v => v);
        }

        public string GetMappingTo(string key, string mappingFrom)
        {
            var compositeKey = (key, mappingFrom);

            return mappings.ContainsKey(compositeKey)
                ? mappings[compositeKey]?.MappingTo
                : null;
        }

    }
}
