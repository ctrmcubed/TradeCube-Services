using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using TradeCube_Services.Helpers;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace TradeCube_Services
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Stopped TradeCubeServices because of exception {ex.Message}");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(o =>
                        {
                            var port = EnvironmentVariableHelper.GetIntEnvironmentVariable("TRADECUBE_SERVICES_HTTPS_PORT");
                            var certificateInfo = X509Helper.CertificateInfo("TRADECUBE_SERVICES_CERT_NAME", "TRADECUBE_SERVICES_CERT_PASSWORD");

                            if (X509Helper.IsValidHttpsConfig(port, certificateInfo))
                            {
                                o.ListenAnyIP(port ?? 0, options => { options.UseHttps(certificateInfo.name, certificateInfo.password); });
                            }
                        })
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.SetMinimumLevel(LogLevel.Trace);
                            logging.AddDebug();
                        })
                        .UseStartup<Startup>()
                        .UseNLog();
                });
    }
}
