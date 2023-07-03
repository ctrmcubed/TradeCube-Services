using Enegen.Managers;
using Enegen.Services;
using Equias.Managers;
using Equias.Services;
using Fidectus.Managers;
using Fidectus.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Serilog;
using Shared.Configuration;
using Shared.Services;
using Shared.Services.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using TradeCube_Services.Extensions;
using TradeCube_Services.Helpers;
using TradeCube_Services.Services;
using TradeCube_Services.Services.ThirdParty.ETRMServices;

try
{
    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
        .AddJsonFile($"appsettings.{environmentName}.json", true, false)
        .Build();

    Log.Logger = new LoggerConfiguration().CreateLogger("/tmp/log/TradeCubeServices.log", configuration);

    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddHttpClient<TradeCubeApiService>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "TradeCube-Services API", Version = "v1", Description = "TradeCube-Services API" });
    });
    
    builder.WebHost.ConfigureKestrel(o =>
    {
        var port = EnvironmentVariableHelper.GetIntEnvironmentVariable("TRADECUBE_SERVICES_HTTPS_PORT");
        var certificateInfo = X509Helper.CertificateInfo("TRADECUBE_SERVICES_CERT_NAME", "TRADECUBE_SERVICES_CERT_PASSWORD");

        if (X509Helper.IsValidHttpsConfig(port, certificateInfo))
        {
            o.ListenAnyIP(port ?? 0, options => { options.UseHttps(certificateInfo.name, certificateInfo.password); });
        }
    });

    // Configuration
    builder.Services
        .AddScoped<IEquiasConfiguration, EquiasConfiguration>()
        .AddScoped<ITradeCubeConfiguration, TradeCubeConfiguration>()
        .AddScoped<IJsReportServerConfiguration, JsReportServerConfiguration>();

    // Managers
    builder.Services
        .AddScoped<IEcvnManager, EcvnManager>()
        .AddScoped<IEquiasManager, EquiasManager>()
        .AddScoped<IFidectusManager, FidectusManager>();

    // Services
    builder.Services
        .AddScoped<ICalculateService, CalculateService>()
        .AddScoped<ICashflowService, CashflowService>()
        .AddScoped<IConfirmationReportService, ConfirmationReportService>()
        .AddScoped<IContactService, ContactService>()
        .AddScoped<ICountryLookupService, CountryLookupService>()
        .AddScoped<ICountryService, CountryService>()
        .AddScoped<IElexonSettlementPeriodService, ElexonSettlementPeriodService>()
        .AddScoped<IEquiasAuthenticationService, EquiasAuthenticationService>()
        .AddScoped<IEquiasMappingService, EquiasMappingService>()
        .AddScoped<IEquiasService, EquiasService>()
        .AddScoped<IFidectusAuthenticationService, FidectusAuthenticationService>()
        .AddScoped<IFidectusService, FidectusService>()
        .AddScoped<IFidectusMappingService, FidectusMappingService>()
        .AddScoped<IFingerprintService, FingerprintService>()
        .AddScoped<IHmacService, HmacService>()
        .AddScoped<IMappingService, MappingService>()
        .AddScoped<IModuleService, ModuleService>()
        .AddScoped<IM7TradeService, M7TradeService>()
        .AddScoped<IEcvnService, EcvnService>()
        .AddScoped<IM7PartyService, M7Im7PartyService>()
        .AddScoped<IPartyService, PartyService>()
        .AddScoped<IProfileService, ProfileService>()
        .AddScoped<IPriceUnitService, PriceUnitService>()
        .AddScoped<IReportTemplateService, ReportTemplateService>()
        .AddScoped<IReportRenderService, ReportRenderService>()
        .AddScoped<ISettingService, SettingService>()
        .AddScoped<ITradeService, TradeService>()
        .AddScoped<ITradeDetailService, TradeDetailService>()
        .AddScoped<ITradeSummaryService, TradeSummaryService>()
        .AddScoped<ITradingBookService, TradingBookService>()
        .AddScoped<IVaultService, VaultService>()
        .AddScoped<IVenueService, VenueService>();

    builder.Services
        .AddSingleton<IRedisService>(_ => new RedisService(configuration.GetSection("RedisConfiguration").Get<RedisConfiguration>()));
    
    builder.Services.AddHealthChecks();

    builder.Services
        .AddMvc()
        .AddXmlSerializerFormatters();
    
    // Add services to the container.
    builder.Services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
    
    builder.Services.AddApiVersioning(v =>
    {
        v.DefaultApiVersion = new ApiVersion(1, 0);
        v.ApiVersionSelector = new CurrentImplementationApiVersionSelector(v);
        v.AssumeDefaultVersionWhenUnspecified = true;
    });
    
    builder.Host.UseSerilog();

    var app = builder.Build();
    
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            swagger.Servers = new List<OpenApiServer>
            {
                new() { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
            };
        });
    });
        
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeCube-Services API v1");
        c.ConfigObject.DeepLinking = true;
    });
    
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Stopped TradeCube-Services because of exception {Message}", ex.Message);
}
finally
{
    Log.CloseAndFlush();
}