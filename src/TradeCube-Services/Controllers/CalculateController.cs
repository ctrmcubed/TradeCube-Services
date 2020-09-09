using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TradeCube_Services.Constants;
using TradeCube_Services.Formula;
using TradeCube_Services.Messages;
using TradeCube_Services.Services;

namespace TradeCube_Services.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    [Route("v{version:apiVersion}/[controller]")]
    public class CalculateController : Controller
    {
        private readonly ICalculateService calculateService;
        private readonly ILogger<CalculateController> logger;

        public CalculateController(ICalculateService calculateService, ILogger<CalculateController> logger)
        {
            this.calculateService = calculateService;
            this.logger = logger;
        }

        [HttpPost]
        public IActionResult Calculate([FromBody] CalculateFormulaRequest calculateFormulaRequest)
        {
            try
            {
                var formulaEvaluatorResponse = calculateService.Calculate(calculateFormulaRequest);

                var apiResponseWrapper = new ApiResponseWrapper<FormulaResponse>
                {
                    Data = new FormulaResponse
                    {
                        Value = formulaEvaluatorResponse.Value
                    },
                    Message = "The Formula calculation process failed",
                    ErrorCount = 1,
                    Errors = new List<string>
                    {
                        formulaEvaluatorResponse.Message
                    }
                };

                return formulaEvaluatorResponse.IsSuccess
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
