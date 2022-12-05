using jsreport.Types;

namespace TradeCube_Services.Services
{
    public interface IReportRenderService
    {
        Task<Report> RenderAsync<T>(string template, string format, T content);
    }
}