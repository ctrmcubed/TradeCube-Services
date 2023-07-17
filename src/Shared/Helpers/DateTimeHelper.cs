using System;
using System.Globalization;
using NodaTime;
using Serilog;

namespace Shared.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly ILogger Logger = Log.ForContext(typeof(DateTimeHelper));

        public static DateTimeZone GetDateTimeZone(string timezone = null)
        {
            if (string.IsNullOrWhiteSpace(timezone))
            {
                return DateTimeZoneProviders.Tzdb.GetSystemDefault();
            }

            var dateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timezone);
            if (dateTimeZone is not null)
            {
                return dateTimeZone;
            }

            Logger.Error("Timezone {Timezone} not found in the Tzdb database, returning UTC", timezone);
            return DateTimeZone.Utc;
        }
        
        public static DateTime GetUtcDateTimeLeniently(DateTime dt, DateTimeZone dateTimeZone) => 
            LocalDateTime.FromDateTime(dt).InZoneLeniently(dateTimeZone).ToDateTimeUtc();
        
        public static DateTime ParseIsoDateTime(string isoDateTime) => 
            DateTime.Parse(isoDateTime, null, DateTimeStyles.RoundtripKind);
        
        public static (bool isAmbiguous, bool isLaterMapping) IsAmbiguous(ZonedDateTime zonedDateTime, DateTimeZone dateTimeZone)
        {
            try
            {
                dateTimeZone.AtStrictly(zonedDateTime.LocalDateTime);

                return (false, false);
            }
            catch (SkippedTimeException)
            {
                return (false, false);
            }
            catch (AmbiguousTimeException ex)
            {
                return (true, zonedDateTime == ex.LaterMapping);
            }
        }        
    }
}