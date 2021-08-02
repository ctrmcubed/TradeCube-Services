using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeCube_Services.Models.ThirdParty.ETRMServices;
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
        private readonly ITradeService tradeService;
        private readonly ILogger<M7Controller> logger;

        public M7Controller(IM7TradeService m7TradeService, ITradeService tradeService, ILogger<M7Controller> logger)
        {
            this.m7TradeService = m7TradeService;
            this.tradeService = tradeService;
            this.logger = logger;
        }

        [HttpPost("Trade")]
        public async Task<IActionResult> Trade([FromHeader] string apiKey, [FromBody] OwnTrade trade)
        {
            try
            {
                var tradeDataObject = await m7TradeService.ConvertTradeAsync(trade, apiKey);
                var saveTrade = await tradeService.PostTradesViaApiKeyAsync(apiKey, new List<TradeDataObject> { tradeDataObject });

                return saveTrade.Status == ApiConstants.SuccessResult
                    ? Ok(tradeDataObject)
                    : BadRequest(saveTrade.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return BadRequest(new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                {
                    Message = ex.Message,
                    Status = ApiConstants.FailedResult
                });
            }
        }
    }
}

