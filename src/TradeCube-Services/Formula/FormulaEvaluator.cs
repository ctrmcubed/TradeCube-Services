using Jint;
using Jint.Native.Json;
using NLog;
using System;
using TradeCube_Services.Serialization;

namespace TradeCube_Services.Formula
{
    public static class FormulaEvaluator
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static FormulaCalculateResponse Evaluate(FormulaEvaluatorRequest formulaEvaluatorRequest)
        {
            try
            {
                var engine = new Engine(options =>
                {
                    options.TimeoutInterval(TimeSpan.FromSeconds(formulaEvaluatorRequest.TimeoutInterval));
                });

                var data = new JsonParser(engine).Parse(formulaEvaluatorRequest.Data);
                var result = engine
                    .Execute(formulaEvaluatorRequest.JavascriptFormula)
                    .GetValue(formulaEvaluatorRequest.MainFunction);

                var jsValue = result.Invoke(data);
                var obj = jsValue.ToObject();
                var serialize = TradeCubeJsonSerializer.Serialize(obj);

                return new FormulaCalculateResponse
                {
                    IsSuccess = true,
                    Value = serialize
                };
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                return new FormulaCalculateResponse
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }
    }
}