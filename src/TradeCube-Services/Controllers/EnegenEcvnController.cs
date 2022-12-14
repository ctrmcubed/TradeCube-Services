using System.Net;
using Enegen.Managers;
using Enegen.Messages;
using Enegen.Services;
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
    public class EnegenEcvnController : Controller
    {
        private readonly IEcvnManager ecvnManager;
        private readonly ILogger<EnegenEcvnController> logger;

        public EnegenEcvnController(IEcvnManager ecvnManager, ILogger<EnegenEcvnController> logger)
        {
            this.ecvnManager = ecvnManager;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Notification([FromHeader] string apiJwtToken, [FromBody] EcvnRequest ecvnRequest)
        {
            try
            {
                var response = await ecvnManager.NotifyAsync(ecvnRequest, apiJwtToken);

                return response.Status == ApiConstants.SuccessResult
                    ? Ok(response)
                    : BadRequest(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return BadRequest(new ApiResponseWrapper<EcvnResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    StatusCode = (int?)HttpStatusCode.BadRequest,
                    Data = new EcvnResponse()
                });
            }
        }
    }
}