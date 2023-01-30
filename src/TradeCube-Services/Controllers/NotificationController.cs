using System.Net;
using Enegen.Managers;
using Enegen.Messages;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Messages;
using Shared.Serialization;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class NotificationController : Controller
    {
        private readonly IEcvnManager ecvnManager;
        private readonly ILogger<NotificationController> logger;

        public NotificationController(IEcvnManager ecvnManager, ILogger<NotificationController> logger)
        {
            this.ecvnManager = ecvnManager;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Notification([FromHeader] string apiJwtToken, [FromBody] WebhookRequest webhookRequest)
        {
            try
            {
                var tradeKey = TradeCubeJsonSerializer.Deserialize<TradeKey>(webhookRequest.Entity);
                var ecvnRequest = new EnegenGenstarEcvnRequest { TradeReference = tradeKey.TradeReference, TradeLeg = tradeKey.TradeLeg };
                var ecvnContext = await ecvnManager.CreateEcvnContext(ecvnRequest, apiJwtToken);
                var ecvn = await ecvnManager.CreateEcvn(ecvnContext, apiJwtToken);
                
                logger.LogInformation("ECVN is {@Ecvn}", ecvn);
                
                var notifyEcvn = await ecvnManager.NotifyEcvn(ecvn, ecvnContext);
                    
                logger.LogInformation("ECVN response {@Response}", notifyEcvn);

                return notifyEcvn.Status == ApiConstants.SuccessResult
                    ? Ok(ecvn)
                    : BadRequest(new ApiResponseWrapper<EnegenGenstarEcvnResponse>
                    {
                        Message = notifyEcvn.Message,
                        Status = ApiConstants.FailedResult,
                        StatusCode = (int?)HttpStatusCode.BadRequest,
                        Data = new EnegenGenstarEcvnResponse()
                    });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return BadRequest(new ApiResponseWrapper<EnegenGenstarEcvnResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    StatusCode = (int?)HttpStatusCode.BadRequest,
                    Data = new EnegenGenstarEcvnResponse()
                });
            }
        }
    }
}