using Equias.Models.BackOfficeServices;
using Shared.DataObjects;
using Shared.Extensions;

namespace Equias.Services
{
    public class EquiasMappingService : IEquiasMappingService
    {
        private const string CmsReportType = "CmsReport";

        public PhysicalTrade MapTrade(TradeDataObject tradeDataObject)
        {
            return new()
            {
                TradeId = MapTradeId(tradeDataObject.TradeReference, tradeDataObject.TradeLeg),
                Uti = tradeDataObject.Uti,
                ProcessInformation = new ProcessInformation
                {
                    ReportingOnBehalfOf = true,
                    EmirReportMode = CmsReportType,
                    RemitReportMode = CmsReportType
                },
                Market = MapCommodity(tradeDataObject.Product.Commodity)
            };
        }

        public static string MapTradeId(string tradeReference, int tradeLeg)
        {
            var reference = tradeReference
                .ToAlphaNumericOnly()
                .GetLast(27)
                .Pad(7, '0');

            var leg = tradeLeg
                .ToString()
                .Pad(3, '0');

            return $"{reference}{leg}";
        }

        private string MapCommodity(CommodityDataObject commodityDataObject)
        {
            return null;
        }

    }
}
