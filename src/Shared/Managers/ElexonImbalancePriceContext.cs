using System;

namespace Shared.Managers;

public class ElexonImbalancePriceContext
{
    public string ApiKey { get; init; }
    public string ElexonApiKey { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public MessageResponseBag MessageResponseBag { get; init; }
}