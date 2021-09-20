using Shared.DataObjects;

namespace Shared.Extensions
{
    public static class TradeDataObjectExtensions
    {
        public static bool WithholdEquiasSubmission(this TradeDataObject tradeDataObject)
        {
            return tradeDataObject?.External?.Equias?.EboWhithhold is not null && tradeDataObject.External.Equias.EboWhithhold.Value;
        }

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

        public static bool IsConfirmationWithheld(this TradeDataObject tradeDataObject)
        {
            return tradeDataObject?.External?.Confirmation?.Withhold is not null && tradeDataObject.External.Confirmation.Withhold.Value;
        }

        public static string ConfirmationDocumentId(this TradeDataObject tradeDataObject)
        {
            return tradeDataObject.External?.Confirmation?.DocumentId;
        }
    }
}