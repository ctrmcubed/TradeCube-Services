using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradeCube_Services.Constants;
using TradeCube_Services.Messages;
using TradeCube_Services.Parameters;
using TradeCube_Services.Services;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class NotificationController : Controller
    {
        private readonly INotificationService notificationService;
        private readonly ILogger<NotificationController> logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            this.notificationService = notificationService;
            this.logger = logger;
        }

        /// <summary>
        /// Example showing how to use a trade webhook by calling a dummy notification service.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Notification([FromHeader] string apiJwtToken, [FromHeader] string subscriberId, [FromBody] WebhookRequest webhookRequest)
        {
            try
            {
                var webhookParameters = new WebhookParameters
                {
                    ApiJwtToken = apiJwtToken,
                    Webhook = webhookRequest.Webhook,
                    Entity = webhookRequest.Entity,
                    EntityType = webhookRequest.EntityType,
                    SubscriberId = subscriberId,
                    Body = webhookRequest.Body,
                    RequestHeaders = Request.Headers.ToDictionary(k => k.Key, v => v)
                };

                var webhookResponse = await notificationService.Notify(webhookParameters);

                return webhookResponse.Status == ApiConstants.SuccessResult
                    ? (IActionResult)Ok(webhookResponse)
                    : BadRequest(webhookResponse);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return BadRequest(new ApiResponseWrapper<WebhookResponse>
                {
                    Message = e.Message,
                    Status = ApiConstants.FailedResult,
                    Data = new WebhookResponse
                    {
                        Webhook = webhookRequest.Webhook
                    }
                });
            }
        }
    }
}