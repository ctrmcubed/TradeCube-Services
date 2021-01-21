using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared.Configuration;
using Shared.Services;
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
            services.AddControllers();
            services.AddHttpClient<TradeCubeApiService>();

            services.AddApiVersioning(v =>
            {
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new ApiVersion(1, 0);
                v.ApiVersionSelector = new CurrentImplementationApiVersionSelector(v);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TradeCube-Services API", Version = "v1", Description = "TradeCube-Services API" });
            });

            // Configuration
            services
                .AddScoped<IEquiasConfiguration, EquiasConfiguration>()
                .AddScoped<ITradeCubeConfiguration, TradeCubeConfiguration>()
                .AddScoped<IJsReportServerConfiguration, JsReportServerConfiguration>();

            // Services
            services
                .AddScoped<ICalculateService, CalculateService>()
                .AddScoped<IConfirmationReportService, ConfirmationReportService>()
                .AddScoped<IContactService, ContactService>()
                .AddScoped<ICountryLookupService, CountryLookupService>()
                .AddScoped<ICountryService, CountryService>()
                .AddScoped<IFingerprintService, FingerprintService>()
                .AddScoped<IMappingService, MappingService>()
                .AddScoped<IM7TradeService, M7TradeService>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<IPartyService, PartyService>()
                .AddScoped<IReportTemplateService, ReportTemplateService>()
                .AddScoped<IReportRenderService, ReportRenderService>()
                .AddScoped<ISettingService, SettingService>()
                .AddScoped<ITradeService, TradeService>()
                .AddScoped<ITradeSummaryService, TradeSummaryService>()
                .AddScoped<ITradingBookService, TradingBookService>()
                .AddScoped<IVenueService, VenueService>();

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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeCube-Services API v1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}
