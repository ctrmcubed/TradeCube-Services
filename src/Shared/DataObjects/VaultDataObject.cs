using System;

namespace Shared.DataObjects
{
    public class VaultDataObject
    {
        public string VaultKey { get; init; }
        public string VaultValue { get; init; }

        public DateTime? ValidFrom { get; init; }
        public DateTime? ValidTo { get; init; }
    }
}
    