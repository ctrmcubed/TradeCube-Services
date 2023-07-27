﻿using Microsoft.AspNetCore.Mvc;
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
    public IActionResult ElexonImbalancePrice([FromHeader] string apiJwtToken, [FromQuery] string elexonAPIKey, 
        string startDate, string endDate, string cube, string dataItem, string layer)
    {
        try
        {
            // var elexonImbalancePriceResponse = elexonImbalancePriceManager.ElexonImbalancePrice(new ElexonImbalancePriceRequest
            // {
            //
            // });
            
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
            // TODO If operating in Cube mode, the user should have CubeDataRW permission.
            // var elexonImbalancePriceResponse = elexonImbalancePriceManager.ElexonImbalancePrice(elexonSettlementPeriodRequest);
            
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
