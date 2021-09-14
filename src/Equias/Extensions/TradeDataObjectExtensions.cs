using Shared.DataObjects;

namespace Equias.Extensions
{
    public static class TradeDataObjectExtensions
    {
        public static string EboActionType(this TradeDataObject tradeDataObject)
        {
            return string.IsNullOrWhiteSpace(tradeDataObject.External?.Equias?.EboTradeId)
                ? null
                : "R";
        }

        public static string EboStatus(this TradeDataObject tradeDataObject)
        {
            return tradeDataObject.External?.Equias?.CmStatus;
        }
    }
}
