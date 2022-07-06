using Equias.Managers;
using Equias.Services;
using Fidectus.Managers;
using Fidectus.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared.Configuration;
using Shared.Services;
using Shared.Services.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;
using TradeCube_Services.Services;
using TradeCube_Services.Services.ThirdParty.ETRMServices;

namespace TradeCube_Services
{
    public class Startup
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TradeCube-Services API", Version = "v1", Description = "TradeCube-Services API" });
            });

            services.AddControllers();
            services.AddHttpClient<TradeCubeApiService>();

            services.AddApiVersioning(v =>
            {
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new ApiVersion(1, 0);
                v.ApiVersionSelector = new CurrentImplementationApiVersionSelector(v);
            });

            services.AddControllersWithViews();

            // Configuration
            services
                .AddScoped<IEquiasConfiguration, EquiasConfiguration>()
                .AddScoped<ITradeCubeConfiguration, TradeCubeConfiguration>()
                .AddScoped<IJsReportServerConfiguration, JsReportServerConfiguration>();

            // Managers
            services
                .AddScoped<IEquiasManager, EquiasManager>()
                .AddScoped<IFidectusManager, FidectusManager>();

            // Services
            services
                .AddScoped<ICalculateService, CalculateService>()
                .AddScoped<ICashflowService, CashflowService>()
                .AddScoped<IConfirmationReportService, ConfirmationReportService>()
                .AddScoped<IContactService, ContactService>()
                .AddScoped<ICountryLookupService, CountryLookupService>()
                .AddScoped<ICountryService, CountryService>()
                .AddScoped<IEquiasAuthenticationService, EquiasAuthenticationService>()
                .AddScoped<IEquiasMappingService, EquiasMappingService>()
                .AddScoped<IEquiasService, EquiasService>()
                .AddScoped<IFidectusAuthenticationService, FidectusAuthenticationService>()
                .AddScoped<IFidectusService, FidectusService>()
                .AddScoped<IFidectusMappingService, FidectusMappingService>()
                .AddScoped<IFingerprintService, FingerprintService>()
                .AddScoped<IMappingService, MappingService>()
                .AddScoped<IM7TradeService, M7TradeService>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<IM7PartyService, M7Im7PartyService>()
                .AddScoped<IPartyService, PartyService>()
                .AddScoped<IProfileService, ProfileService>()
                .AddScoped<IPriceUnitService, PriceUnitService>()
                .AddScoped<IReportTemplateService, ReportTemplateService>()
                .AddScoped<IReportRenderService, ReportRenderService>()
                .AddScoped<ISettingService, SettingService>()
                .AddScoped<ITradeService, TradeService>()
                .AddScoped<ITradeSummaryService, TradeSummaryService>()
                .AddScoped<ITradingBookService, TradingBookService>()
                .AddScoped<IVaultService, VaultService>()
                .AddScoped<IVenueService, VenueService>();

            services
                .AddSingleton<IRedisService>(_ =>
                    new RedisService(configuration.GetSection("RedisConfiguration").Get<RedisConfiguration>()));

            services.AddHealthChecks();

            services
                .AddMvc()
                .AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeCube-Services API v1");
            });

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
