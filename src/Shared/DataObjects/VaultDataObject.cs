using System;

namespace Shared.DataObjects
{
    public class VaultDataObject
    {
        public string VaultKey { get; set; }
        public string VaultValue { get; set; }

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}