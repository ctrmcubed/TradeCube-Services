using jsreport.Local;
using jsreport.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TradeCube_Services.Constants;
using TradeCube_Services.Exceptions;

namespace TradeCube_Services.Services
{
    public class ReportRenderService : IReportRenderService
    {
        private readonly ILogger<ReportRenderService> logger;

        public ReportRenderService(ILogger<ReportRenderService> logger)
        {
            this.logger = logger;
        }

        public async Task<Report> Render<T>(string template, string format, T content)
        {
            try
            {
                var rs = new LocalReporting()
                    .UseBinary(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                        jsreport.Binary.JsReportBinary.GetBinary() :
                        jsreport.Binary.Linux.JsReportBinary.GetBinary())
                    .AsUtility()
                    .Create();

                var recipe = MapFormatToRecipe(format);

                logger.LogDebug($"Format: {format}");
                logger.LogDebug($"Recipe: {recipe}");
                logger.LogDebug($"Content: {template}");
                logger.LogDebug($"Data: {content}");

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
