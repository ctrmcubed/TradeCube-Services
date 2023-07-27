using System;

namespace Shared.Exceptions;

public class ElexonImbalancePriceException : Exception
{
    public ElexonImbalancePriceException(string message) : base(message)
    {
    }
}