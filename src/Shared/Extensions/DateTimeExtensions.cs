using System;
using System.Globalization;

namespace Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FromIso8601DateTime(this string iso)
        {
            return DateTime.Parse(iso, null, DateTimeStyles.RoundtripKind);
        }
    }
}
