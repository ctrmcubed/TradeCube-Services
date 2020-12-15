using System;

namespace Equias.Models.BackOfficeServices
{
    public class ExerciseSchedule
    {
        public DateTime DeliveryStartTimestamp { get; set; }
        public DateTime DeliveryEndTimestamp { get; set; }
        public DateTime ExerciseDateTimestamp { get; set; }
        public decimal ExerciseTimeZone { get; set; }
    }
}