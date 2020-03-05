using System.Threading.Tasks;
using TradeCube_Services.Messages;
using TradeCube_Services.Models;

namespace TradeCube_Services.Services
{
    public interface IReportTemplateService
    {
        Task<ApiResponseWrapper<ReportTemplate>> ReportTemplate(string templateType);
    }
}