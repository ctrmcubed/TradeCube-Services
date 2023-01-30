using NodaTime;
using System;
using Serilog;

namespace Shared.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly ILogger Logger = Log.ForContext(typeof(DateTimeHelper));

        public static DateTimeZone GetTimeZone(string timezone)
        {
            try
            {
                return DateTimeZoneProviders.Tzdb[timezone];
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Timezone {Timezone} not found in Tzdb database", timezone);
                throw;
            }
        }
    }
}