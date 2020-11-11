using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TradeCube_Services.Configuration;
using TradeCube_Services.Services;
using TradeCube_Services.Services.ThirdParty.Enegen;

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
            services.AddHttpClient();
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
                .AddScoped<ITradeCubeConfiguration, TradeCubeConfiguration>()
                .AddScoped<IJsReportServerConfiguration, JsReportServerConfiguration>();

            // Services
            services
                .AddScoped<ICalculateService, CalculateService>()
                .AddScoped<IConfirmationReportService, ConfirmationReportService>()
                .AddScoped<ICountryLookupService, CountryLookupService>()
                .AddScoped<ICountryService, CountryService>()
                .AddScoped<IEnegenAtomService, EnegenAtomService>()
                .AddScoped<IEnegenGenstarService, EnegenGenstarService>()
                .AddScoped<INotificationService, NotificationService>()
                .AddScoped<IReportTemplateService, ReportTemplateService>()
                .AddScoped<IReportRenderService, ReportRenderService>()
                .AddScoped<ITradeService, TradeService>()
                .AddScoped<ITradeProfileService, TradeProfileService>()
                .AddScoped<IVaultService, VaultService>();

            services.AddHealthChecks();
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
