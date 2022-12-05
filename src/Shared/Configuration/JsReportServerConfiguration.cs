using System;

namespace Shared.Configuration
{
    public class JsReportServerConfiguration : ConfigurationBase, IJsReportServerConfiguration
    {
        public string ReportServerDomain { get; init; }
        public string ReportServerPort { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }

        public JsReportServerConfiguration()
        {
            ReportServerDomain = Environment.GetEnvironmentVariable("JSREPORT_DOMAIN");
            ReportServerPort = Environment.GetEnvironmentVariable("JSREPORT_PORT");
            
            Username = Environment.GetEnvironmentVariable("JSREPORT_USERNAME");
            Password = Environment.GetEnvironmentVariable("JSREPORT_PASSWORD");
        }

        public string WebApiUrl() => $"{ReportServerDomain}{Port(ReportServerPort)}/";
    }
}
