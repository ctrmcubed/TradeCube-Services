using System;
using System.Collections.Generic;

namespace Shared.DataObjects
{
    public class ProductCascade
    {
        public DateTime? CascadeDateTime { get; set; }
        public List<string> CascadeProducts { get; set; }
    }
}