using NLog;
using NodaTime;
using System;

namespace Shared.Helpers
{
    public class DateTimeHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static DateTimeZone GetTimeZone(string timezone)
        {
            try
            {
                return DateTimeZoneProviders.Tzdb[timezone];
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Timezone {timezone} not found in Tzdb database");
                throw;
            }
        }
    }
}
