using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using TradeCube_Services.Constants;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class CalculateController : Controller
    {
        private readonly ILogger<CalculateController> logger;

        public CalculateController(ILogger<CalculateController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Calculate([FromBody] CalculateFormulaRequest calculateFormulaRequest)
        {
            try
            {
                logger.LogInformation(calculateFormulaRequest.ToString());

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return BadRequest(new ApiResponseWrapper<WebServiceResponse> { Message = e.Message, Status = ApiConstants.FailedResult });
            }
        }
    }
}
