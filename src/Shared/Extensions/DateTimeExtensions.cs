using System;
using System.Globalization;

namespace Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToIso8601DateTime(this DateTime dt, bool ms = true)
        {
            return ms
                ? dt.ToString("yyyy-MM-dd'T'HH:mm:ssK", CultureInfo.InvariantCulture)
                : dt.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}