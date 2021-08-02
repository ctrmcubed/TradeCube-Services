using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Messages;
using System;
using System.Threading.Tasks;
using TradeCube_Services.Parameters;
using TradeCube_Services.Services;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ConfirmationReportController : Controller
    {
        private readonly IConfirmationReportService confirmationReportService;
        private readonly ILogger<ConfirmationReportController> logger;

        public ConfirmationReportController(IConfirmationReportService confirmationReportService, ILogger<ConfirmationReportController> logger)
        {
            this.confirmationReportService = confirmationReportService;
            this.logger = logger;
        }

        [HttpPost("ConfirmationReport")]
        public async Task<IActionResult> ConfirmationReport([FromHeader] string apiJwtToken, [FromBody] WebServiceRequest webServiceRequest)
        {
            try
            {
                var confirmationReportParameters = new ConfirmationReportParameters
                {
                    ApiJwtToken = apiJwtToken,
                    ActionName = webServiceRequest.ActionName,
                    Template = TemplateConstants.ConfirmationTemplate,
                    Format = webServiceRequest.Format,
                    TradeReferences = webServiceRequest.Entities
                };

                var confirmationReport = await confirmationReportService.CreateReportAsync(confirmationReportParameters);

                return confirmationReport.Status == ApiConstants.SuccessResult
                    ? Ok(confirmationReport)
                    : BadRequest(confirmationReport);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponseWrapper<WebServiceResponse> { Message = ex.Message, Status = ApiConstants.FailedResult });
            }
        }
    }
}