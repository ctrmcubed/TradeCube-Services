using System;
using Shared.Messages;
using Shared.Serialization;
using TradeCube_Services.Formula;
using TradeCube_Services.Helpers;

namespace TradeCube_Services.Services
{
    public class CalculateService : ICalculateService
    {
        public FormulaCalculateResponse Calculate(CalculateFormulaRequest calculateFormulaRequest)
        {
            try
            {
                var formula = StringHelper.Base64ToString(calculateFormulaRequest.Formula);

                var formulaDataWrapper = new FormulaDataWrapper
                {
                    Data = calculateFormulaRequest.Data
                };

                var json = TradeCubeJsonSerializer.Serialize(formulaDataWrapper);
                var formulaEvaluatorRequest = new FormulaEvaluatorRequest(json, formula, "main")
                {
                    TimeoutInterval = calculateFormulaRequest.TimeoutSeconds
                };

                return FormulaEvaluator.Evaluate(formulaEvaluatorRequest);
            }
            catch (Exception ex)
            {
                return new FormulaCalculateResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
