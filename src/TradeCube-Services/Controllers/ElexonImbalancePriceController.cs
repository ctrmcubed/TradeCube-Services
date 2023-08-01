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
public class ElexonImbalancePriceController : Controller
{
    private readonly IElexonImbalancePriceManager elexonImbalancePriceManager;
    private readonly ILogger<ElexonImbalancePriceController> logger;

    public ElexonImbalancePriceController(IElexonImbalancePriceManager elexonImbalancePriceManager, ILogger<ElexonImbalancePriceController> logger)
    {
        this.elexonImbalancePriceManager = elexonImbalancePriceManager;
        this.logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWrapper<IEnumerable<ElexonImbalancePriceRequestResponseItem>>), 200)]
    public async Task<IActionResult> ElexonImbalancePrice([FromHeader] string apiKey, [FromQuery] string elexonAPIKey, 
        string mode, string startDate, string endDate, string cube, string dataItem, string layer)
    {
        var elexonImbalancePriceRequest = new ElexonImbalancePriceRequest
        {
            ElexonApiKey = elexonAPIKey,
            Mode = mode,
            StartDate = startDate,
            EndDate = endDate,
            Cube = cube,
            DataItem = dataItem,
            Layer = layer
        };
            
        return await ActionResult(apiKey, elexonImbalancePriceRequest);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWrapper<IEnumerable<ElexonImbalancePriceRequestResponseItem>>), 200)]
    public async Task<IActionResult> ElexonImbalancePrice([FromHeader] string apiKey, [FromBody] ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        return await ActionResult(apiKey, elexonImbalancePriceRequest);
    }

    [NonAction]
    private async Task<IActionResult> ActionResult(string apiKey, ElexonImbalancePriceRequest elexonImbalancePriceRequest)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return Unauthorized();
        }

        try
        {
            var imbalancePriceRequest = new ElexonImbalancePriceRequest
            {
                ApiKey = apiKey,
                ElexonApiKey = elexonImbalancePriceRequest.ElexonApiKey,
                Mode = elexonImbalancePriceRequest.Mode,
                StartDate = elexonImbalancePriceRequest.StartDate,
                EndDate = elexonImbalancePriceRequest.EndDate,
                Cube = elexonImbalancePriceRequest.Cube,
                DataItem = elexonImbalancePriceRequest.DataItem,
                Layer = elexonImbalancePriceRequest.Layer
            };
            
            var elexonImbalancePriceResponse = await elexonImbalancePriceManager.ElexonImbalancePriceWithCdb(imbalancePriceRequest);

            return elexonImbalancePriceResponse.IsSuccess()
                ? Ok(new ApiResponseWrapper<ElexonImbalancePriceRequestResponseItem>
                {
                    Status = ApiConstants.SuccessResult,
                    Message = elexonImbalancePriceResponse.Message
                })
                : Ok(new ApiResponseWrapper<ElexonImbalancePriceRequestResponseItem>
                {
                    Status = ApiConstants.FailedResult,
                    Message = elexonImbalancePriceResponse.Message
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

