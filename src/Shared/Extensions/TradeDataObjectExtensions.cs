using Shared.DataObjects;

namespace Shared.Extensions
{
    public static class TradeDataObjectExtensions
    {
        public static bool IsVoid(this TradeDataObject tradeDataObject) => 
            tradeDataObject.TradeStatus == TradeConstants.TradeStatusVoid;

        public static bool WithholdEquiasSubmission(this TradeDataObject tradeDataObject) => 
            tradeDataObject?.External?.Equias?.EboWhithhold is not null && tradeDataObject.External.Equias.EboWhithhold.Value;

        public static string EboActionType(this TradeDataObject tradeDataObject) =>
            string.IsNullOrWhiteSpace(tradeDataObject.External?.Equias?.EboTradeId)
                ? null
                : "R";

        public static string EboStatus(this TradeDataObject tradeDataObject) => 
            tradeDataObject.External?.Equias?.CmStatus;

        public static bool IsConfirmationWithheld(this TradeDataObject tradeDataObject) => 
            tradeDataObject?.External?.Confirmation?.Withhold is not null && tradeDataObject.External.Confirmation.Withhold.Value;

        public static string ConfirmationDocumentId(this TradeDataObject tradeDataObject) => 
            tradeDataObject.External?.Confirmation?.DocumentId;
    }
}