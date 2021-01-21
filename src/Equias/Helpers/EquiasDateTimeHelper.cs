using NodaTime;
using System;
using System.Globalization;

namespace Equias.Helpers
{
    public static class EquiasDateTimeHelper
    {
        public static string FormatDateTimeWithOffset(DateTime dt, DateTimeZone dateTimeZone)
        {
            var offset = dateTimeZone.GetUtcOffset(Instant.FromDateTimeUtc(dt));
            var timeSpan = offset.ToTimeSpan();
            var formattedDateTime = dt.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);

            return $"{formattedDateTime}{Math.Sign(timeSpan.Hours)}{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}";
        }
    }
}
