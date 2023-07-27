namespace Shared.Types.Elexon;

public class DerivedSystemWideDataRequest
{
    public string Url { get; init; }
    public string FromSettlementDate { get; init; }
    public string ToSettlementDate { get; init; }
    public string SettlementPeriod { get; init; }
    public string ServiceType { get; init; }
    public string ElexonApiKey { get; init; }
}