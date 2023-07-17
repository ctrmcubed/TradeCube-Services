using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Messages;

namespace Shared.Managers;

public class UkPowerManager : IUkPowerManager
{
    private readonly ILogger<UkPowerManager> logger;

    public UkPowerManager(ILogger<UkPowerManager> logger)
    {
        this.logger = logger;
    } 
    
    public ElexonSettlementPeriodResponse ComputeElexonSettlementPeriods(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest)
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
                    UtcStartDateTime = utcStartDateTime,
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
                Data = SettlementPeriods(CreateContext(ValidateRequest(elexonSettlementPeriodRequest)))
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

    private ElexonSettlementPeriodRequest ValidateRequest(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest)
    {
        ArgumentNullException.ThrowIfNull(elexonSettlementPeriodRequest);

        if (string.IsNullOrWhiteSpace(elexonSettlementPeriodRequest.UtcStartDateTime))
        {
            throw new ElexonSettlementPeriodException("The mandatory field 'UTCStartDateTime' has not been supplied.");
        }

        return elexonSettlementPeriodRequest;
    }

    private ElexonSettlementPeriodContext CreateContext(ElexonSettlementPeriodRequest elexonSettlementPeriodRequest)
    {
        var utcStartDateTime = DateTimeHelper.ParseIsoDateTime(elexonSettlementPeriodRequest.UtcStartDateTime);
        var utcEndDateTime = string.IsNullOrWhiteSpace(elexonSettlementPeriodRequest.UtcEndDateTime)
            ? utcStartDateTime
            : DateTimeHelper.ParseIsoDateTime(elexonSettlementPeriodRequest.UtcEndDateTime);

        if (utcEndDateTime < utcStartDateTime)
        {
            throw new ElexonSettlementPeriodException("The UTCEndDateTime is before the UTCStartDateTime.");
        }

        return new ElexonSettlementPeriodContext()
        {
            UtcStartDateTime = utcStartDateTime,
            UtcEndDateTime = utcEndDateTime
        };
    }
}