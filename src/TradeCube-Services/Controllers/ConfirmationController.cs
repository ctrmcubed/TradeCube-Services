using Fidectus.Managers;
using Fidectus.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Messages;
using System;
using System.Threading.Tasks;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ConfirmationController : Controller
    {
        private readonly IFidectusManager fidectusManager;
        private readonly ILogger<ConfirmationController> logger;

        public ConfirmationController(IFidectusManager fidectusManager, ILogger<ConfirmationController> logger)
        {
            this.fidectusManager = fidectusManager;
            this.logger = logger;
        }

        [HttpPost("{key}")]
        public async Task<IActionResult> Confirmation([FromHeader] string apiJwtToken, string key, [FromQuery(Name = "tradeLeg")] int? leg)
        {
            try
            {
                return !string.IsNullOrWhiteSpace(key) && leg.HasValue
                    ? Json(await fidectusManager.SendConfirmationAsync(key, leg.Value, apiJwtToken))
                    : BadRequest();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return BadRequest(new ApiResponseWrapper<SendConfirmationResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    Data = new SendConfirmationResponse
                    {
                        Message = ex.Message
                    }
                });
            }
        }

    }
}
