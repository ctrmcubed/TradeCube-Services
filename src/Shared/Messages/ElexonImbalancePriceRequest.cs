namespace Shared.Messages;

public class ElexonImbalancePriceRequest
{
    public string ElexonAPIKey { get; init; }
    public string StartDate { get; init; }
    public string EndDate { get; init; }
    public string Cube { get; init; }
    public string DataItem { get; init; }
    public string Layer { get; init; }
}