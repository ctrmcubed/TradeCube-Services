using Fidectus.Managers;
using Fidectus.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class FidectusController : Controller
    {
        private readonly IFidectusManager fidectusManager;
        private readonly ILogger<FidectusController> logger;

        public FidectusController(IFidectusManager fidectusManager, ILogger<FidectusController> logger)
        {
            this.fidectusManager = fidectusManager;
            this.logger = logger;
        }

        [HttpPost("Confirmation")]
        public async Task<IActionResult> Confirmation([FromHeader] string apiJwtToken, [FromQuery] string tradeReference, [FromQuery] int tradeLeg)
        {
            try
            {
                return string.IsNullOrWhiteSpace(tradeReference)
                    ? BadRequest()
                    : Json(await fidectusManager.ConfirmAsync(tradeReference, tradeLeg, apiJwtToken, await fidectusManager.GetFidectusConfiguration(apiJwtToken)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponseWrapper<ConfirmationResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    Data = new ConfirmationResponse
                    {
                        Message = ex.Message
                    }
                });
            }
        }

        [HttpDelete("Confirmation")]
        public async Task<IActionResult> DeleteConfirmation([FromHeader] string apiJwtToken, [FromQuery] string tradeReference, [FromQuery] int tradeLeg)
        {
            try
            {
                return string.IsNullOrWhiteSpace(tradeReference)
                    ? BadRequest()
                    : Json(await fidectusManager.CancelAsync(tradeReference, tradeLeg, apiJwtToken, await fidectusManager.GetFidectusConfiguration(apiJwtToken)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponseWrapper<ConfirmationResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    Data = new ConfirmationResponse
                    {
                        Message = ex.Message
                    }
                });
            }
        }

        [HttpPost("ConfirmationResult")]
        public async Task<IActionResult> ConfirmationResult([FromHeader] string apiJwtToken, [FromBody] IEnumerable<TradeKey> tradeKeys)
        {
            try
            {
                var confirmationResultResponses = await fidectusManager.BoxResults(tradeKeys, apiJwtToken, await fidectusManager.GetFidectusConfiguration(apiJwtToken)).ToListAsync();

                return Json(new ConfirmationResultResponses
                {
                    Responses = confirmationResultResponses
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponseWrapper<ConfirmationResultResponses>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    Data = new ConfirmationResultResponses
                    {
                        Message = ex.Message
                    }
                });
            }
        }
    }
}
