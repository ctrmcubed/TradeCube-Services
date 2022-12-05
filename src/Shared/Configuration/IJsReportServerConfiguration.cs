namespace Shared.Configuration
{
    public interface IJsReportServerConfiguration
    {
        string ReportServerDomain { get; init; }
        string ReportServerPort { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        
        string WebApiUrl();
    }
}