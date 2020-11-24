using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using TradeCube_Services.Constants;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Messages;
using TradeCube_Services.Services;
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

        [HttpPost]
        public async Task<IActionResult> Trade([FromHeader] string apiKey, [FromBody] XElement trade)
        {
            try
            {
                var tradeDataObject = await m7TradeService.ConvertTradeAsync(trade, apiKey);
                var saveTrade = await tradeService.SaveTradesAsync(apiKey, new List<TradeDataObject> { tradeDataObject });

                return saveTrade.Status == ApiConstants.SuccessResult
                    ? (IActionResult)Ok(tradeDataObject)
                    : BadRequest(saveTrade.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return BadRequest(new ApiResponseWrapper<IEnumerable<TradeDataObject>>
                {
                    Message = e.Message,
                    Status = ApiConstants.FailedResult
                });
            }
        }
    }
}
