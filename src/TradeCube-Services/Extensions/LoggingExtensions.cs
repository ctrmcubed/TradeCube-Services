using Serilog;
using Serilog.Core;
using Shared.DataAccess;
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
        try
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

            var loggingCollection = Environment.GetEnvironmentVariable("SCAFELL_LOGGING_COLLECTION");

            if (string.IsNullOrWhiteSpace(loggingCollection))
            {
                return file.CreateLogger();
            }

            file.WriteTo.MongoDBBson(cfg =>
            {
                cfg.SetMongoUrl(new Tenant().ScafellConnectionString);
                cfg.SetCollectionName(loggingCollection);
                cfg.SetBatchPeriod(TimeSpan.FromSeconds(2));
                cfg.SetExpireTTL(TimeSpan.FromDays(90));
            });

            return file.CreateLogger();
        }
        catch (Exception)
        {
            return CreateSimpleLogger(loggerConfiguration, fileName);
        }
    }
}