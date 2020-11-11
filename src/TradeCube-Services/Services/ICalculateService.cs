using TradeCube_Services.Formula;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services
{
    public interface ICalculateService
    {
        FormulaCalculateResponse Calculate(CalculateFormulaRequest calculateFormulaRequest);
    }
}