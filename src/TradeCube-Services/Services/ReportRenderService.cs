using jsreport.Client;
using jsreport.Types;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Constants;
using System;
using System.Threading.Tasks;
using TradeCube_Services.Exceptions;

namespace TradeCube_Services.Services
{
    public class ReportRenderService : IReportRenderService
    {
        private readonly IJsReportServerConfiguration jsReportServerConfiguration;
        private readonly ILogger<ReportRenderService> logger;

        public ReportRenderService(IJsReportServerConfiguration jsReportServerConfiguration, ILogger<ReportRenderService> logger)
        {
            this.jsReportServerConfiguration = jsReportServerConfiguration;
            this.logger = logger;
        }

        public async Task<Report> RenderAsync<T>(string template, string format, T content)
        {
            try
            {
                var url = jsReportServerConfiguration.WebApiUrl();
                var rs = new ReportingService(url);
                var recipe = MapFormatToRecipe(format);

                logger.LogInformation($"Attempting to render report. URL: {url}, Recipe: {recipe}");

                var report = await rs.RenderAsync(new RenderRequest
                {
                    Template = new Template
                    {
                        Recipe = recipe,
                        Engine = Engine.Handlebars,
                        Content = template
                    },
                    Data = new
                    {
                        trades = content
                    }
                });

                return report;
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                throw;
            }
        }

        private Recipe MapFormatToRecipe(string format)
        {
            return format switch
            {
                FormatConstants.Pdf => Recipe.ChromePdf,
                FormatConstants.Html => Recipe.Html,
                FormatConstants.Xlsx => Recipe.HtmlToXlsx,
                FormatConstants.Txt => Recipe.HtmlToText,
                _ => throw new RecipeException($"Recipe for {format} is not supported for this service")
            };
        }
    }
}
