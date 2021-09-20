using NodaTime;
using System;
using System.Globalization;

namespace Fidectus.Helpers
{
    public static class FidectusDateTimeHelper
    {
        public static string FormatDateTimeWithOffset(DateTime dt, DateTimeZone dateTimeZone)
        {
            var fromDateTimeUtc = Instant.FromDateTimeUtc(dt);
            var timeSpan = dateTimeZone.GetUtcOffset(fromDateTimeUtc).ToTimeSpan();
            var localTime = fromDateTimeUtc.InZone(dateTimeZone).LocalDateTime;
            var formattedDateTime = localTime.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);
            var sign = Math.Sign(timeSpan.Hours);
            var signSymbol = sign >= 0
                ? "+"
                : "-";

            return $"{formattedDateTime}{signSymbol}{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}";
        }
    }
}
