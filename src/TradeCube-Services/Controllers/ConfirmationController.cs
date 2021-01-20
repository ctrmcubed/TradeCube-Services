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
    public class ConfirmationController : Controller
    {
        private readonly IConfirmationReportService confirmationReportService;
        private readonly ILogger<ConfirmationController> logger;

        public ConfirmationController(IConfirmationReportService confirmationReportService, ILogger<ConfirmationController> logger)
        {
            this.confirmationReportService = confirmationReportService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Confirmation([FromHeader] string apiJwtToken, [FromBody] WebServiceRequest webServiceRequest)
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
                    ? (IActionResult)Ok(confirmationReport)
                    : BadRequest(confirmationReport);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return BadRequest(new ApiResponseWrapper<WebServiceResponse> { Message = e.Message, Status = ApiConstants.FailedResult });
            }
        }
    }
}