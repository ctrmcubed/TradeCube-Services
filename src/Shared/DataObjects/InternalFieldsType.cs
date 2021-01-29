using System;

namespace Shared.DataObjects
{
    public class InternalFieldsType
    {
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastUpdatedDateTime { get; set; }
        public bool? DemonstrationData { get; set; }
        public bool? System { get; set; }
    }
}