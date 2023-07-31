using System;
using Shared.Constants;

namespace Shared.Managers;

public class ElexonImbalancePriceContext
{
    public string ElexonApiKey { get; init; }
    public string Mode { get; init; }
    public string Cube { get; init; }
    public string DataItem { get; init; }
    public string Layer { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    
    public MessageResponseBag MessageResponseBag { get; init; }

    public bool IsModeStandalone() => 
        Mode == ElexonImbalancePriceConstants.ModeStandalone;
    
    public bool IsModeCube() => 
        Mode == ElexonImbalancePriceConstants.ModeCube;
}