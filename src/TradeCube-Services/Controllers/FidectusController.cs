using System.Net;
using Fidectus.Managers;
using Fidectus.Messages;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Messages;

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
        public async Task<IActionResult> Confirmation([FromHeader] string apiJwtToken, [FromBody] TradeKey tradeKey)
        {
            try
            {
                if (tradeKey is null)
                {
                    return BadRequest();
                }

                var confirmationResponse = (await fidectusManager.ConfirmAsync(tradeKey, apiJwtToken, await fidectusManager.GetFidectusConfiguration(apiJwtToken)));

                if (confirmationResponse.IsSuccess())
                {
                    return Json(confirmationResponse);
                }

                return BadRequest(confirmationResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return BadRequest(new ApiResponseWrapper<ConfirmationResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    StatusCode = (int?)HttpStatusCode.BadRequest,
                    Data = new ConfirmationResponse
                    {
                        Message = ex.Message
                    }
                });
            }
        }

        [HttpDelete("Confirmation")]
        public async Task<IActionResult> DeleteConfirmation([FromHeader] string apiJwtToken, [FromBody] TradeKey tradeKey)
        {
            try
            {
                if (tradeKey is null)
                {
                    return BadRequest();
                }

                var confirmationResponse = await fidectusManager.CancelAsync(tradeKey, apiJwtToken, await fidectusManager.GetFidectusConfiguration(apiJwtToken));

                if (confirmationResponse.IsSuccess())
                {
                    return Json(confirmationResponse);
                }

                return BadRequest(confirmationResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return BadRequest(new ApiResponseWrapper<ConfirmationResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    StatusCode = (int?)HttpStatusCode.BadRequest,
                    Data = new ConfirmationResponse
                    {
                        Message = ex.Message
                    }
                });
            }
        }

        [HttpPost("ConfirmationResult")]
        public async Task<IActionResult> ConfirmationResult([FromHeader] string apiJwtToken, [FromBody] TradeKey tradeKey)
        {
            try
            {
                if (tradeKey is null)
                {
                    return BadRequest();
                }

                return Json(await fidectusManager.BoxResult(tradeKey, apiJwtToken, await fidectusManager.GetFidectusConfiguration(apiJwtToken)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return BadRequest(new ApiResponseWrapper<ConfirmationResultResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    StatusCode = (int?)HttpStatusCode.BadRequest,
                    Data = new ConfirmationResultResponse
                    {
                        Message = ex.Message
                    }
                });
            }
        }
    }
}
