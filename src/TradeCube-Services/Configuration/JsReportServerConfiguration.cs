using System;

namespace TradeCube_Services.Configuration
{
    public class JsReportServerConfiguration : ConfigurationBase, IJsReportServerConfiguration
    {
        public string ReportServerDomain { get; set; }
        public string ReportServerPort { get; set; }

        public JsReportServerConfiguration()
        {
            ReportServerDomain = Environment.GetEnvironmentVariable("JSREPORT_DOMAIN");
            ReportServerPort = Environment.GetEnvironmentVariable("JSREPORT_PORT");
        }

        public string WebApiUrl() => $"{ReportServerDomain}{Port(ReportServerPort)}/";
    }
}
