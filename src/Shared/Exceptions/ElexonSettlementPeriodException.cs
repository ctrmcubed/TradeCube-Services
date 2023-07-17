using System;

namespace Shared.Exceptions;

public class ElexonSettlementPeriodException : Exception
{
    public ElexonSettlementPeriodException(string message) : base(message)
    {
    }
}