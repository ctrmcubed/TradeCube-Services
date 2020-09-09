namespace TradeCube_Services.Formula
{
    public class FormulaEvaluatorRequest
    {
        public string Data { get; }
        public string JavascriptFormula { get; }
        public string MainFunction { get; }

        public int LimitMemory { get; set; }
        public int TimeoutInterval { get; set; }

        public FormulaEvaluatorRequest(string data, string javascriptFormula, string mainFunction)
        {
            Data = data;
            JavascriptFormula = javascriptFormula;
            MainFunction = mainFunction;
        }
    }
}