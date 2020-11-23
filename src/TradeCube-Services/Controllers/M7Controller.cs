using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using TradeCube_Services.Constants;
using TradeCube_Services.Messages;
using TradeCube_Services.Services.ThirdParty.ETRMServices;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class M7Controller : Controller
    {
        private readonly IM7TradeService m7TradeService;
        private readonly ILogger<M7Controller> logger;

        public M7Controller(IM7TradeService m7TradeService, ILogger<M7Controller> logger)
        {
            this.m7TradeService = m7TradeService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Trade([FromHeader] string apiKey, [FromBody] XElement trade)
        {
            try
            {
                //var webhookParameters = new WebhookParameters
                //{
                //    ApiJwtToken = apiJwtToken,
                //    Webhook = webhookRequest.Webhook,
                //    Entity = webhookRequest.Entity,
                //    EntityType = webhookRequest.EntityType,
                //    SubscriberId = subscriberId,
                //    Body = webhookRequest.Body,
                //    RequestHeaders = Request.Headers.ToDictionary(k => k.Key, v => v)
                //};

                var tradeDataObject = await m7TradeService.ConvertTrade(trade, apiKey);

                return Ok();

                //return tradeDataObject.Status == ApiConstants.SuccessResult
                //    ? (IActionResult)Ok(tradeDataObject)
                //    : BadRequest(tradeDataObject);
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
                        //Webhook = webhookRequest.Webhook
                    }
                });
            }
        }
    }
}
