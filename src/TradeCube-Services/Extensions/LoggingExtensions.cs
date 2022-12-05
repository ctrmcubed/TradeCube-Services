using Serilog;
using Serilog.Core;
using ILogger = Serilog.ILogger;

namespace TradeCube_Services.Extensions;

public static class LoggingExtensions
{
    public static Logger CreateSimpleLogger(this LoggerConfiguration loggerConfiguration, string fileName)
    {
        return loggerConfiguration
            .MinimumLevel.Debug()
            .WriteTo.File(fileName)
            .WriteTo.Console()
            .CreateLogger();
    }

    public static ILogger CreateLogger(this LoggerConfiguration loggerConfiguration, string fileName, IConfigurationRoot configuration)
    {
        var file = loggerConfiguration
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .WriteTo.Console()
            .WriteTo.File(fileName, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10);

        var logStashUriEnvVar = Environment.GetEnvironmentVariable("SCAFELL_LOGSTASH_URI") ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(logStashUriEnvVar))
        {
            file.WriteTo.Http(requestUri: new Uri(logStashUriEnvVar).ToString(), queueLimitBytes: null);
        }

        return file.CreateLogger();
    }
}