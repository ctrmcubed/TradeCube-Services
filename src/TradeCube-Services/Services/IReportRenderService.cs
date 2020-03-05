using jsreport.Types;
using System.Threading.Tasks;

namespace TradeCube_Services.Services
{
    public interface IReportRenderService
    {
        Task<Report> Render<T>(string template, string format, T content);
    }
}