using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Managers;
using Shared.Messages;

namespace TradeCube_Services.Controllers;

[ApiVersion("1.0")]
[Produces("application/json")]
[Route("[controller]")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class ElexonSettlementPeriodController : Controller
{
    private readonly IUkPowerManager ukPowerManager;
    private readonly ILogger<ElexonSettlementPeriodController> logger;

    public ElexonSettlementPeriodController(IUkPowerManager ukPowerManager, ILogger<ElexonSettlementPeriodController> logger)
    {
        this.ukPowerManager = ukPowerManager;
        this.logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWrapper<IEnumerable<ElexonSettlementPeriodResponseItem>>), 200)]
    public IActionResult ElexonSettlementPeriod([FromBody] ElexonSettlementPeriodRequest elexonSettlementPeriodRequest)
    {
        IActionResult ComputeSettlementPeriods()
        {
            var settlementPeriodResponse = ukPowerManager.ComputeElexonSettlementPeriods(elexonSettlementPeriodRequest);

            var responseWrapper = new ApiResponseWrapper<IEnumerable<ElexonSettlementPeriodResponseItem>>
            {
                Data = settlementPeriodResponse.Data,
                Message = settlementPeriodResponse.Message
            };

            return settlementPeriodResponse.IsSuccess()
                ? Ok(responseWrapper)
                : BadRequest(responseWrapper);
        }

        try
        {
            return ComputeSettlementPeriods();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            return BadRequest(new ApiResponseWrapper<IEnumerable<ElexonSettlementPeriodResponseItem>>
            {
                Status = ApiConstants.FailedResult, Message = ex.Message
            });
        }
    }
}