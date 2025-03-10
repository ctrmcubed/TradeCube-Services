﻿using System.Net;
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
    public class EnegenECVNController : Controller
    {
        private readonly IEcvnManager ecvnManager;
        private readonly ILogger<EnegenECVNController> logger;

        public EnegenECVNController(IEcvnManager ecvnManager, ILogger<EnegenECVNController> logger)
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
                var enegenGenstarEcvnExternalRequest = await ecvnManager.CreateEcvnRequest(ecvnContext, apiJwtToken);
                
                logger.LogInformation("ECVN request: {@Ecvn}", enegenGenstarEcvnExternalRequest);

                if (!enegenGenstarEcvnExternalRequest.IsValid())
                {
                    logger.LogInformation("ECVN request was not valid ({ValidationMessage})", enegenGenstarEcvnExternalRequest.ValidationMessage);
                    
                    return BadRequest(new ApiResponseWrapper<EnegenGenstarEcvnResponse>
                    {
                        Message = enegenGenstarEcvnExternalRequest.ValidationMessage,
                        Status = ApiConstants.FailedResult,
                        StatusCode = (int?)HttpStatusCode.BadRequest,
                        Data = new EnegenGenstarEcvnResponse()
                    });
                }
                
                var notifyEcvn = await ecvnManager.NotifyEcvn(enegenGenstarEcvnExternalRequest, ecvnContext);
                    
                logger.LogInformation("ECVN response: {@Response}", notifyEcvn);

                return notifyEcvn.Status == ApiConstants.SuccessResult
                    ? Ok(enegenGenstarEcvnExternalRequest)
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