using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Messages;

namespace Shared.Managers;

public class ElexonSettlementPeriodManager : IElexonSettlementPeriodManager
{
    private readonly ILogger<ElexonSettlementPeriodManager> logger;

    public ElexonSettlementPeriodManager(ILogger<ElexonSettlementPeriodManager> logger)
    {
        this.logger = logger;
    }
    
    public ElexonSettlementPeriodResponse ElexonSettlementPeriods(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest)
    {
        DateTime ComputeIntervalFunc(DateTime time, DateTime dateTime) => time.AddMinutes(30);

        IEnumerable<ElexonSettlementPeriodResponseItem> SettlementPeriods(ElexonSettlementPeriodContext ctx)
        {
            var dateTimeZone = DateTimeHelper.GetDateTimeZone("Europe/London");
            var timeSeriesUtc = TimeSeriesHelper.CreateTimeSeriesUtc(ctx.UtcStartDateTime, ctx.UtcEndDateTime, ComputeIntervalFunc, null, true, dateTimeZone);

            foreach (var periodType in timeSeriesUtc)
            {
                var localDateOnly = periodType.LocalStart.Date;
                var utcDateTimeLeniently = DateTimeHelper.GetUtcDateTimeLeniently(localDateOnly, dateTimeZone);
                var minutesDiff = periodType.UtcStart.Subtract(utcDateTimeLeniently).TotalMinutes;
                var halfHours = (int)minutesDiff / 30;
                var utcStartDateTime = periodType.UtcStart.ToIso8601DateTime();
                var settlementDate = localDateOnly.ToString("yyyy-MM-dd");
                
                yield return new ElexonSettlementPeriodResponseItem
                {
                    StartDateTimeUtc = utcStartDateTime,
                    SettlementDate = settlementDate,
                    SettlementPeriod = halfHours + 1
                };
            }
        }

        try
        {
            return new ElexonSettlementPeriodResponse
            {
                Status = ApiConstants.SuccessResult,
                Data = SettlementPeriods(CreateContext(ValidateRequest(elexonSettlementPeriodRequest))).NullIfEmpty()
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            return new ElexonSettlementPeriodResponse
            {
                Status = ApiConstants.FailedResult,
                Message   = ex.Message
            };
        }
    }
    
    private static ElexonSettlementPeriodContext CreateContext(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest)
    {
        var utcStartDateTime = DateTimeHelper.ParseIsoDateTime(elexonSettlementPeriodRequest.StartDateTimeUtc);
        var utcEndDateTime = string.IsNullOrWhiteSpace(elexonSettlementPeriodRequest.EndDateTimeUtc)
            ? utcStartDateTime
            : DateTimeHelper.ParseIsoDateTime(elexonSettlementPeriodRequest.EndDateTimeUtc);

        if (utcEndDateTime < utcStartDateTime)
        {
            throw new ElexonSettlementPeriodException("The EndDateTimeUTC is before the StartDateTimeUTC.");
        }

        var roundedStart = RoundToNearestHalfHour(utcStartDateTime);
        var roundedEnd = RoundToNearestHalfHour(utcEndDateTime);
        
        return new ElexonSettlementPeriodContext
        {
            UtcStartDateTime = roundedStart,
            UtcEndDateTime = roundedEnd
        };
    }

    private static DateTime RoundToNearestHalfHour(DateTime dt) =>
        dt.Minute < 30
            ? new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, dt.Kind)
            : new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 30, 0, dt.Kind);

    private static ElexonSettlementPeriodRequest ValidateRequest(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest)
    {
        ArgumentNullException.ThrowIfNull(elexonSettlementPeriodRequest);

        if (string.IsNullOrWhiteSpace(elexonSettlementPeriodRequest.StartDateTimeUtc))
        {
            throw new ElexonSettlementPeriodException("The mandatory field 'UTCStartDateTime' has not been supplied.");
        }

        return elexonSettlementPeriodRequest;
    }    
}