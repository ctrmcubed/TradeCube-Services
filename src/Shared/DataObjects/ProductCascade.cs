using System;
using System.Collections.Generic;

namespace Shared.DataObjects
{
    public class ProductCascade
    {
        public DateTime? CascadeDateTime { get; set; }
        public IEnumerable<string> CascadeProducts { get; set; }
    }
}