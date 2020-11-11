using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TradeCube_Services.Constants;
using TradeCube_Services.Messages;
using TradeCube_Services.Parameters;
using TradeCube_Services.Services.ThirdParty.Enegen;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class EnegenAtomController : ControllerBase
    {
        private readonly IEnegenAtomService enegenAtomService;
        private readonly ILogger<EnegenAtomController> logger;

        public EnegenAtomController(IEnegenAtomService enegenAtomService, ILogger<EnegenAtomController> logger)
        {
            this.enegenAtomService = enegenAtomService;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Trade([FromHeader] string apiJwtToken, [FromBody] WebServiceRequest webServiceRequest)
        {
            try
            {
                var enegenAtomTradeParameters = new EnegenAtomTradeParameters
                {
                    Url = Environment.GetEnvironmentVariable("ENEGEN_ATOM_URL"),
                    ApiJwtToken = apiJwtToken,
                    WebService = webServiceRequest.WebService,
                    TradeReferences = webServiceRequest.Entities
                };

                var apiResponseWrapper = await enegenAtomService.Trade(enegenAtomTradeParameters);

                return apiResponseWrapper.Status == ApiConstants.SuccessResult
                    ? (IActionResult)Ok(apiResponseWrapper)
                    : BadRequest(apiResponseWrapper);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return BadRequest(new ApiResponseWrapper<WebServiceResponse> { Message = e.Message, Status = ApiConstants.FailedResult });
            }
        }
    }
}