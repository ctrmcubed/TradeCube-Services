using Equias.Managers;
using Equias.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class EboController : Controller
    {
        private readonly IEquiasManager equiasManager;
        private readonly ILogger<EboController> logger;

        public EboController(IEquiasManager equiasManager, ILogger<EboController> logger)
        {
            this.equiasManager = equiasManager;
            this.logger = logger;
        }


        [HttpPost("TradeStatus")]
        public async Task<IActionResult> TradeStatus([FromHeader] string apiJwtToken, [FromBody] IEnumerable<TradeKey> tradeKeys)
        {
            try
            {
                return Json(await equiasManager.TradeStatusAsync(tradeKeys,  apiJwtToken));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponseWrapper<EboGetTradeStatusResponse> { Message = ex.Message, Status = ApiConstants.FailedResult });
            }
        }

        [HttpPost("PhysicalTrade")]
        public async Task<IActionResult> PhysicalTrade([FromHeader] string apiJwtToken, [FromQuery] string tradeReference, [FromQuery] int tradeLeg)
        {
            try
            {
                return Json(await equiasManager.CreatePhysicalTradeAsync(tradeReference, tradeLeg, apiJwtToken));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return BadRequest(new ApiResponseWrapper<EboTradeResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    Data = new EboTradeResponse
                    {
                        Message = ex.Message
                    }
                });
            }
        }

        [HttpPost("CancelTrade")]
        public async Task<IActionResult> CancelTrade([FromHeader] string apiJwtToken, [FromQuery] string tradeReference, [FromQuery] int tradeLeg)
        {
            try
            {
                return Json(await equiasManager.CancelTradeAsync(tradeReference, tradeLeg, apiJwtToken));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return BadRequest(new ApiResponseWrapper<EboTradeResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    Data = new EboTradeResponse
                    {
                        Message = ex.Message
                    }
                });
            }
        }
    }
}