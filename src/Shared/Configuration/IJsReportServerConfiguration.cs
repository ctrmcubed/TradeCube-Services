namespace Shared.Configuration
{
    public interface IJsReportServerConfiguration
    {
        string ReportServerDomain { get; set; }
        string ReportServerPort { get; set; }
        string WebApiUrl();
    }
}