namespace Enegen.Types;

public class SettlementPeriodVolume
{
    public int ElexonSettlementPeriod { get; init; }
    public string ElexonSettlementDate { get; init; }
    public decimal Volume { get; init; }
}
