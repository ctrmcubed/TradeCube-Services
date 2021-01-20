namespace Shared.Configuration
{
    public interface IEquiasConfiguration
    {
        string ApiDomain { get; }
        string RequestTokenUri { get; }
        string AddPhysicalTradeUri { get; }
    }
}