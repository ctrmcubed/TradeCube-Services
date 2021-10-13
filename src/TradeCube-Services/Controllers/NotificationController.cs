﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Messages;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

                var webhookResponse = await notificationService.NotifyAsync(webhookParameters);

                return webhookResponse.Status == ApiConstants.SuccessResult
                    ? Ok(webhookResponse)
                    : BadRequest(webhookResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponseWrapper<WebhookResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    StatusCode = (int?)HttpStatusCode.BadRequest,
                    Data = new WebhookResponse
                    {
                        Webhook = webhookRequest.Webhook
                    }
                });
            }
        }
    }
}