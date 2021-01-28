namespace Shared.Configuration
{
    public interface IEquiasConfiguration
    {
        string ApiDomain { get; }
        string RequestTokenUri { get; }
        string GetTradeStatusUri { get; }
        string AddPhysicalTradeUri { get; }
        string ModifyPhysicalTradeUri { get; }
    }
}