using Equias.Managers;
using Equias.Messages;
using Equias.Services;
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
    public class EboController : Controller
    {
        private readonly IEquiasManager equiasManager;
        private readonly IEquiasAuthenticationService equiasAuthenticationService;
        private readonly ILogger<EboController> logger;

        public EboController(IEquiasManager equiasManager, IEquiasAuthenticationService equiasAuthenticationService, ILogger<EboController> logger)
        {
            this.equiasManager = equiasManager;
            this.equiasAuthenticationService = equiasAuthenticationService;
            this.logger = logger;
        }

        [HttpPost("TradeStatus")]
        public async Task<IActionResult> TradeStatus([FromHeader] string apiJwtToken, [FromBody] EboGetTradeStatusRequest eboGetTradeStatusRequest)
        {
            try
            {
                var requestTokenRequest = await equiasManager.GetAuthenticationToken(apiJwtToken);
                var requestTokenResponse = await equiasAuthenticationService.GetAuthenticationToken(requestTokenRequest);
                var eboGetTradeStatusResponse = await equiasManager.EboGetTradeStatus(eboGetTradeStatusRequest.TradeIds, requestTokenResponse);

                return Json(eboGetTradeStatusResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponseWrapper<WebServiceResponse> { Message = ex.Message, Status = ApiConstants.FailedResult });
            }
        }

        [HttpPost("PhysicalTrade")]
        public async Task<IActionResult> PhysicalTrade([FromHeader] string apiJwtToken, [FromBody] EboAddPhysicalTradeRequest eboAddPhysicalTradeRequest)
        {
            try
            {
                var requestTokenRequest = await equiasManager.GetAuthenticationToken(apiJwtToken);
                var requestTokenResponse = await equiasAuthenticationService.GetAuthenticationToken(requestTokenRequest);
                var tradeDataObject = await equiasManager.GetTradeAsync(eboAddPhysicalTradeRequest.TradeReference, eboAddPhysicalTradeRequest.TradeLeg, apiJwtToken);
                var tradeIds = new List<string> { EquiasService.MapTradeId(eboAddPhysicalTradeRequest.TradeReference, eboAddPhysicalTradeRequest.TradeLeg) };
                var eboGetTradeStatusResponse = await equiasManager.EboGetTradeStatus(tradeIds, requestTokenResponse);
                var updateTradePreSubmission = await equiasManager.UpdateTradePreSubmission(eboGetTradeStatusResponse, tradeDataObject, apiJwtToken);
                var physicalTrade = await equiasManager.CreatePhysicalTrade(tradeDataObject, apiJwtToken);

                if (eboGetTradeStatusResponse.States.SingleOrDefault()?.TradeVersion == null)
                {
                    var eboAddPhysicalTradeResponse = await equiasManager.AddPhysicalTrade(physicalTrade, requestTokenResponse);
                    var addTradePostSubmission = await equiasManager.UpdateTradePostSubmission(eboAddPhysicalTradeResponse, tradeDataObject, apiJwtToken);

                    logger.LogInformation(addTradePostSubmission.Status);

                    return Json(eboAddPhysicalTradeResponse);
                }

                // Mutation!
                physicalTrade.ActionType = eboAddPhysicalTradeRequest.ActionType;

                var eboModifyPhysicalTradeResponse = await equiasManager.ModifyPhysicalTrade(physicalTrade, requestTokenResponse);
                var modifyTradePostSubmission = await equiasManager.UpdateTradePostSubmission(eboModifyPhysicalTradeResponse, tradeDataObject, apiJwtToken);

                logger.LogInformation(modifyTradePostSubmission.Status);

                return Json(eboModifyPhysicalTradeResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                return BadRequest(new ApiResponseWrapper<EboPhysicalTradeResponse>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult,
                    Data = new EboPhysicalTradeResponse
                    {
                        Message = ex.Message
                    }
                });
            }
        }
    }
}