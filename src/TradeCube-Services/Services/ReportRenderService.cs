using jsreport.Local;
using jsreport.Types;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TradeCube_Services.Constants;
using TradeCube_Services.Exceptions;

namespace TradeCube_Services.Services
{
    public class ReportRenderService : IReportRenderService
    {
        public async Task<Report> Render<T>(string template, string format, T content)
        {
            var rs = new LocalReporting()
                .UseBinary(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
                    jsreport.Binary.JsReportBinary.GetBinary() :
                    jsreport.Binary.Linux.JsReportBinary.GetBinary())
                .AsUtility()
                .Create();

            var report = await rs.RenderAsync(new RenderRequest
            {
                Template = new Template
                {
                    Recipe = MapFormatToRecipe(format),
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
