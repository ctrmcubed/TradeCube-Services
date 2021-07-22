namespace Shared.Configuration
{
    public interface IFidectusConfiguration
    {
        string ApiDomain { get; }
        string RequestTokenUri { get; }
    }
}