using Shared.Messages;
using TradeCube_Services.Formula;

namespace TradeCube_Services.Services
{
    public interface ICalculateService
    {
        FormulaCalculateResponse Calculate(CalculateFormulaRequest calculateFormulaRequest);
    }
}