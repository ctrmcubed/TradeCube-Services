﻿using Equias.Constants;
using Equias.Managers;
using Equias.Messages;
using Equias.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.Extensions;
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

                if (tradeDataObject.WithholdEquiasSubmission())
                {
                    tradeDataObject.External.Equias.EboSubmissionStatus = EquiasConstants.StatusWithheld;

                    var saveTradeWithheld = await equiasManager.SaveTrade(tradeDataObject, apiJwtToken);

                    logger.LogInformation($"Withheld Trade updated (EboSubmissionStatus={EquiasConstants.StatusWithheld}), result: {saveTradeWithheld.IsSuccessStatusCode}");

                    return Json(new EboPhysicalTradeResponse());
                }

                var tradeIds = new List<string> { EquiasService.MapTradeId(eboAddPhysicalTradeRequest.TradeReference, eboAddPhysicalTradeRequest.TradeLeg) };
                var eboGetTradeStatusResponse = await equiasManager.EboGetTradeStatus(tradeIds, requestTokenResponse);
                var updateTradePreSubmission = equiasManager.SetTradePreSubmission(eboGetTradeStatusResponse, tradeDataObject);
                var savePreSubmission = await equiasManager.SaveTrade(updateTradePreSubmission, apiJwtToken);

                logger.LogInformation($"Pre-submission Trade updated (EboSubmissionStatus={tradeDataObject.External.Equias.EboSubmissionStatus}), result: {savePreSubmission.IsSuccessStatusCode}");

                var physicalTrade = await equiasManager.CreatePhysicalTrade(tradeDataObject, apiJwtToken);

                if (eboGetTradeStatusResponse.States.SingleOrDefault()?.TradeVersion == null)
                {
                    var eboAddPhysicalTradeResponse = await equiasManager.AddPhysicalTrade(physicalTrade, requestTokenResponse);

                    if (eboAddPhysicalTradeResponse.IsSuccessStatusCode)
                    {
                        var addTradePostSubmission = equiasManager.SetTradePostSubmission(eboAddPhysicalTradeResponse, tradeDataObject);
                        var savePostSubmissionAdd = await equiasManager.SaveTrade(addTradePostSubmission, apiJwtToken);

                        logger.LogInformation($"Add physical Trade updated (EboSubmissionStatus={tradeDataObject.External.Equias.EboSubmissionStatus}), result: {savePostSubmissionAdd.IsSuccessStatusCode}");

                        return Json(eboAddPhysicalTradeResponse);
                    }

                    logger.LogInformation($"Add physical Trade failed result: {eboAddPhysicalTradeResponse.Message}");

                    return BadRequest(new ApiResponseWrapper<EboPhysicalTradeResponse>
                    {
                        Message = eboAddPhysicalTradeResponse.Message,
                        Status = ApiConstants.FailedResult,
                        Data = new EboPhysicalTradeResponse
                        {
                            Message = eboAddPhysicalTradeResponse.Message
                        }
                    });
                }

                // Mutation!
                physicalTrade.ActionType = eboAddPhysicalTradeRequest.ActionType;

                var eboModifyPhysicalTradeResponse = await equiasManager.ModifyPhysicalTrade(physicalTrade, requestTokenResponse);
                if (eboModifyPhysicalTradeResponse.IsSuccessStatusCode)
                {
                    var modifyTradePostSubmission = equiasManager.SetTradePostSubmission(eboModifyPhysicalTradeResponse, tradeDataObject);
                    var savePostSubmissionModify = await equiasManager.SaveTrade(modifyTradePostSubmission, apiJwtToken);

                    logger.LogInformation($"Modify physical Trade updated (EboSubmissionStatus={tradeDataObject.External.Equias.EboSubmissionStatus}), result: {savePostSubmissionModify.IsSuccessStatusCode}");

                    return Json(eboModifyPhysicalTradeResponse);
                }

                logger.LogInformation($"Modify physical Trade failed result: {eboModifyPhysicalTradeResponse.Message}");

                return BadRequest(new ApiResponseWrapper<EboPhysicalTradeResponse>
                {
                    Message = eboModifyPhysicalTradeResponse.Message,
                    Status = ApiConstants.FailedResult,
                    Data = new EboPhysicalTradeResponse
                    {
                        Message = eboModifyPhysicalTradeResponse.Message
                    }
                });
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