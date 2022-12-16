namespace Enegen.Services;

public class EcvnContext
{
    public string TradeReference { get; init; }
    public int TradeLeg { get; init; }
    public string EnegenEcvnUrlSetting { get; init; }
    public string EnegenEcvnAppIdSetting { get; init; }
    public string EnegenPskVaultValue{ get; init; }
    public string ApiJwtToken { get; init; }
}