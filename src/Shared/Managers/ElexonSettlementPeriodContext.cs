using System;

namespace Shared.Managers;

public class ElexonSettlementPeriodContext
{
    public DateTime UtcStartDateTime { get; init; }
    public DateTime UtcEndDateTime { get; init; }
}