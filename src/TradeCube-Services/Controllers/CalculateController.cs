using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Messages;
using TradeCube_Services.Formula;
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
                    Message = formulaEvaluatorResponse.Message,
                    ErrorCount = formulaEvaluatorResponse.IsSuccess ? 0 : 1,
                    Errors = formulaEvaluatorResponse.IsSuccess
                        ? new List<string>()
                        : new List<string>
                        {
                            formulaEvaluatorResponse.Message
                        }
                };

                return formulaEvaluatorResponse.IsSuccess
                    ? Ok(apiResponseWrapper)
                    : BadRequest(apiResponseWrapper);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);
                return BadRequest(new ApiResponseWrapper<WebServiceResponse> { Message = ex.Message, Status = ApiConstants.FailedResult });
            }
        }
    }
}