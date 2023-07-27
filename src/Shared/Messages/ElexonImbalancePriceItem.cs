namespace Shared.Messages;

public class ElexonImbalancePriceItem
{
    public string SettlementDate { get; init; }
    public int SettlementPeriod { get; init; }
    public double ImbalancePrice { get; init; }
    public string StartDateTimeUTC { get; init; } 
}