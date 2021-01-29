using Shared.DataObjects;

namespace Shared.Extensions
{
    public static class TradeDataObjectExtensions
    {
        public static bool WithholdEquiasSubmission(this TradeDataObject tradeDataObject)
        {
            return tradeDataObject?.External?.Equias?.EboWhithhold != null && tradeDataObject.External.Equias.EboWhithhold.Value;
        }
    }
}