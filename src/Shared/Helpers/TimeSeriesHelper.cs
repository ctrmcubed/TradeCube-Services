using System;
using System.Collections.Generic;
using NodaTime;

namespace Shared.Helpers;

public static class TimeSeriesHelper
{
    public static IEnumerable<PeriodType> CreateTimeSeriesUtc(DateTime utcStart, DateTime utcEnd,
        Func<DateTime, DateTime, DateTime> computeIntervalFunc, int? dayMinuteOffset, bool isInclusive,
        DateTimeZone dateTimeZone)
    {
        var currentDateTime = utcStart;

        while (currentDateTime < utcEnd || (isInclusive && currentDateTime <= utcEnd))
        {
            var periodType = ComputePeriodTypeUtc(currentDateTime, dayMinuteOffset, dateTimeZone);
            var utcEndDateTime = computeIntervalFunc(currentDateTime, currentDateTime).ToUniversalTime();

            currentDateTime = utcEndDateTime;

            yield return periodType;
        }
    }
    
    public static PeriodType ComputePeriodTypeUtc(DateTime utcDt, int? dayMinuteOffset, DateTimeZone dateTimeZone)
    {
        var startInstant = Instant.FromDateTimeUtc(utcDt.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(utcDt, DateTimeKind.Utc)
            : utcDt);
        
        var zonedDateTime = startInstant.InZone(dateTimeZone);
        var awareStart = zonedDateTime.ToDateTimeUnspecified();
            
        var offsetStart = dayMinuteOffset.HasValue
            ? awareStart.AddMinutes(-dayMinuteOffset.Value).Date
            : awareStart.Date;
            
        var startLocal = zonedDateTime.ToDateTimeUnspecified();
        var (_, isLaterMapping) = DateTimeHelper.IsAmbiguous(zonedDateTime, dateTimeZone);

        return new PeriodType
        {
            UtcStart = utcDt, 
            LocalStart = startLocal, 
            OffsetStart = offsetStart, 
            DstFlag = isLaterMapping
        };
    }    
}