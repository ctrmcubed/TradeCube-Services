using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Messages;

namespace TradeCube_Services.Controllers;

[ApiVersion("1.0")]
[Produces("application/json")]
[Route("[controller]")]
[Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class ElexonImbalancePriceController : Controller
{
    private readonly ILogger<ElexonImbalancePriceController> logger;

    public ElexonImbalancePriceController(ILogger<ElexonImbalancePriceController> logger)
    {
        this.logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWrapper<IEnumerable<ElexonImbalancePriceRequestResponseItem>>), 200)]
    public IActionResult ElexonImbalancePrice([FromHeader] string apiJwtToken, [FromQuery] string elexonAPIKey, 
        string startDate, string endDate, string cube, string dataItem, string layer)
    {
        try
        {
            return Ok(new ApiResponseWrapper<ElexonImbalancePriceRequestResponseItem>
            {
                Status = ApiConstants.SuccessResult
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            return BadRequest(new ApiResponseWrapper<ElexonImbalancePriceRequestResponseItem>
            {
                Message = ex.Message,
                Status = ApiConstants.FailedResult
            });
        }
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWrapper<IEnumerable<ElexonImbalancePriceRequestResponseItem>>), 200)]
    public IActionResult ElexonImbalancePrice([FromHeader] string apiJwtToken, [FromBody] ElexonImbalancePriceRequest elexonSettlementPeriodRequest)
    {
        try
        {
            return Ok(new ApiResponseWrapper<ElexonImbalancePriceRequestResponseItem>
            {
                Status = ApiConstants.SuccessResult
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Message}", ex.Message);
            return BadRequest(new ApiResponseWrapper<ElexonImbalancePriceRequestResponseItem>
            {
                Message = ex.Message,
                Status = ApiConstants.FailedResult
            });
        }
    }
}

