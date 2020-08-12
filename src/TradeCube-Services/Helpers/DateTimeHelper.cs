using NodaTime;
using System;

namespace TradeCube_Services.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GetLocalDateTime(DateTime dt, string timezone)
        {
            var instant = Instant.FromDateTimeUtc(dt);
            var tz = DateTimeZoneProviders.Tzdb[timezone];
            var zdt = instant.InZone(tz);
            var localTime = zdt.ToDateTimeUnspecified();

            return localTime;
        }

        public static int PeriodNumber(DateTime dt, double periodLengthInMinutes)
        {
            return 1 + (int)((dt.Hour + dt.Minute / 60.0) / periodLengthInMinutes);
        }
    }
}