using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeCube_Services.Constants;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;
using TradeCube_Services.Parameters;

namespace TradeCube_Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ITradeService tradeService;
        private readonly ILogger<NotificationService> logger;

        public NotificationService(ITradeService tradeService, ILogger<NotificationService> logger)
        {
            this.tradeService = tradeService;
            this.logger = logger;
        }

        public async Task<ApiResponseWrapper<WebhookResponse>> NotifyAsync(WebhookParameters webhookParameters)
        {
            try
            {
                return webhookParameters.EntityType switch
                {
                    WebhookConstants.TradingWebhook => await Notification(webhookParameters),
                    _ => new ApiResponseWrapper<WebhookResponse>
                    {
                        Status = ApiConstants.FailedResult,
                        Message = $"Unknown webhook entity type ({webhookParameters.EntityType})",
                        Data = new WebhookResponse
                        {
                            Entity = webhookParameters.Entity,
                            Webhook = webhookParameters.Webhook
                        }
                    }
                };
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new ApiResponseWrapper<WebhookResponse>
                {
                    Status = ApiConstants.FailedResult,
                    Message = e.Message,
                    Data = new WebhookResponse
                    {
                        Webhook = webhookParameters.Webhook
                    }
                };
            }
        }

        private async Task<ApiResponseWrapper<WebhookResponse>> Notification(WebhookParameters webhookParameters)
        {
            ApiResponseWrapper<WebhookResponse> CreateResponse(string status, string message = null)
            {
                return new ApiResponseWrapper<WebhookResponse>
                {
                    Status = status,
                    Message = message,
                    Data = new WebhookResponse
                    {
                        Webhook = webhookParameters.Webhook
                    }
                };
            }

            try
            {
                var apiJwtToken = webhookParameters.ApiJwtToken;
                var request = new TradeRequest { TradeReferences = new List<string> { webhookParameters.Entity } };

                // We don't specify a trade leg so we could get multiple trades back
                var trades = await tradeService.GetTradesAsync(apiJwtToken, request);

                if (trades.Status == ApiConstants.SuccessResult)
                {
                    if (trades.RecordCount == 0)
                    {
                        logger.LogError("Trade not found, not notifying");
                        return CreateResponse(ApiConstants.FailedResult, "Trade not found, not notifying");
                    }

                    if (trades.RecordCount > 1)
                    {
                        // For this example, bail out if multiple trade legs
                        logger.LogError("Trade has multiple legs, not notifying");
                        return CreateResponse(ApiConstants.FailedResult, "Trade has multiple legs, not notifying");
                    }

                    logger.LogInformation($"Notifying trades, subscriberId: {webhookParameters.SubscriberId}");

                    // Call dummy 'Notify()' service
                    var allSuccess = trades.Data.Select(NotifyAsync);
                    var result = allSuccess.Any(s => false)
                        ? ApiConstants.FailedResult
                        : ApiConstants.SuccessResult;

                    return CreateResponse(result);
                }

                logger.LogError("Error calling Trade API", trades.Message);
                return CreateResponse(ApiConstants.FailedResult, $"Error calling Trade API ({trades.Message})");
            }
            catch (Exception e)
            {
                logger.LogError($"Error notifying trade {webhookParameters.Entity}", e.Message);
                return CreateResponse(ApiConstants.FailedResult, $"Error notifying trade {webhookParameters.Entity}");
            }
        }

        private bool NotifyAsync(TradeDataObject tradeDataObject)
        {
            logger.LogInformation($"Notifying trade {tradeDataObject.TradeReference}...");

            return true;
        }
    }
}

