using System;

namespace Shared.Messages
{
    public class ProfileBase
    {
        public DateTime UtcStartDateTime { get; set; }
        public DateTime UtcEndDateTime { get; set; }
        public decimal Value { get; set; }
    }
}