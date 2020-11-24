using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using TradeCube_Services.Constants;
using TradeCube_Services.Messages;
using TradeCube_Services.Models;

namespace TradeCube_Services.Services
{
    public class ReportTemplateService : IReportTemplateService
    {
        private readonly IHostEnvironment hostEnvironment;
        private readonly ILogger<ReportTemplateService> logger;

        public ReportTemplateService(IHostEnvironment hostEnvironment, ILogger<ReportTemplateService> logger)
        {
            this.hostEnvironment = hostEnvironment;
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<ReportTemplate>> ReportTemplateAsync(string templateType)
        {
            var separatorChar = Path.DirectorySeparatorChar;

            async Task<ApiResponseWrapper<ReportTemplate>> ReadTemplateAsync(string fileName)
            {
                try
                {
                    var file = await ReadFileAsync($"{hostEnvironment.ContentRootPath}{separatorChar}Templates{separatorChar}{fileName}");

                    return new ApiResponseWrapper<ReportTemplate>(ApiConstants.SuccessResult, new ReportTemplate { Html = file });
                }
                catch (Exception e)
                {
                    logger.LogError($"The template could not be read ({templateType})", e.Message);
                    return new ApiResponseWrapper<ReportTemplate>(ApiConstants.FailedResult, new ReportTemplate()) { Message = e.Message };
                }
            }

            return templateType switch
            {
                TemplateConstants.ConfirmationTemplate => await ReadTemplateAsync(TemplateConstants.ConfirmationTemplateFile),
                _ => new ApiResponseWrapper<ReportTemplate> { Status = ApiConstants.FailedResult, Message = $"Unknown template type ({templateType})" }
            };
        }

        private async Task<string> ReadFileAsync(string filename)
        {
            try
            {
                using var sr = new StreamReader(filename);
                return await sr.ReadToEndAsync();
            }
            catch (IOException e)
            {
                logger.LogError($"The file could not be read ({filename})", e.Message);
                throw;
            }
        }
    }
}
