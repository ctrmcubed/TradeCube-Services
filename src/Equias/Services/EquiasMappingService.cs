using Equias.Helpers;
using Equias.Models.BackOfficeServices;
using NodaTime;
using Shared.DataObjects;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Managers;
using Shared.Messages;
using Shared.Services;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Equias.Services
{
    public class EquiasMappingService : IEquiasMappingService
    {
        private readonly IMappingService mappingService;
        private const string CmsReportType = "CmsReport";
        private MappingManager mappingManager;

        public EquiasMappingService(IMappingService mappingService)
        {
            this.mappingService = mappingService;
        }

        public EquiasMappingService SetMappingManager(MappingManager mapping)
        {
            mappingManager = mapping;
            return this;
        }

        public async Task<IEnumerable<MappingDataObject>> GetMappingsAsync(string apiJwtToken)
        {
            return (await mappingService.GetMappingsAsync(apiJwtToken))?.Data;
        }

        public PhysicalTrade MapTrade(TradeDataObject tradeDataObject, TradeSummaryResponse tradeSummaryResponse, IEnumerable<CashflowType> cashflowTypes,
            IEnumerable<ProfileResponse> profileResponses)
        {
            if (tradeDataObject == null)
            {
                throw new DataException("Trade is null");
            }

            if (tradeDataObject.Product?.Commodity?.Timezone == null)
            {
                throw new DataException("Trade's timezone is null");
            }

            var timezone = DateTimeHelper.GetTimeZone(tradeDataObject.Product?.Commodity?.Timezone);

            return new PhysicalTrade
            {
                TradeId = MapTradeId(tradeDataObject.TradeReference, tradeDataObject.TradeLeg),
                Uti = tradeDataObject.Uti,
                ProcessInformation = new ProcessInformation
                {
                    ReportingOnBehalfOf = true,
                    EmirReportMode = CmsReportType,
                    RemitReportMode = CmsReportType
                },
                Market = MapCommodityToMarket(tradeDataObject.Product?.Commodity),
                Commodity = MapCommodityToCommodity(tradeDataObject.Product?.Commodity?.Commodity),
                TransactionType = MapContractTypeToTransactionType(tradeDataObject.Product?.ContractType),
                DeliveryPointArea = tradeDataObject.Product?.Commodity?.DeliveryArea?.Eic,
                BuyerParty = MapEicLei(tradeDataObject.Buyer?.Eic?.Eic, tradeDataObject.Buyer?.Lei?.Lei, "BuyerParty"),
                SellerParty = MapEicLei(tradeDataObject.Seller?.Eic?.Eic, tradeDataObject.Seller?.Lei?.Lei, "SellerParty"),
                BeneficiaryId = MapEicLei(tradeDataObject.Beneficiary?.Eic?.Eic, tradeDataObject.Beneficiary?.Lei?.Lei, "BeneficiaryId", false),
                Intragroup = MapInternalToIntragroup(tradeDataObject.Buyer?.Internal, tradeDataObject.Seller?.Internal),
                LoadType = MapShapeDescriptionToLoadType(tradeDataObject.Product?.ShapeDescription, "Custom"),
                Agreement = MapContractAgreementToAgreement(tradeDataObject.Contract?.AgreementType?.AgreementType),
                TotalVolume = tradeSummaryResponse?.TotalVolume,
                TotalVolumeUnit = MapEnergyUnitToVolumeUnit(tradeDataObject.Quantity?.QuantityUnit?.EnergyUnit?.EnergyUnit),
                TradeExecutionTimestamp = EquiasDateTimeHelper.FormatDateTimeWithOffset(tradeDataObject.TradeDateTime, timezone),
                CapacityUnit = MapQuantityUnitToCapacityUnit(tradeDataObject.Quantity?.QuantityUnit?.QuantityUnit),
                PriceUnit = MapPriceUnit(tradeDataObject.Price?.PriceUnit),
                TotalContractValue = tradeSummaryResponse?.TotalValue,
                SettlementCurrency = tradeSummaryResponse?.TotalValueCurrency,
                SettlementDates = cashflowTypes?.Select(d => d.SettlementDate.ToIso8601DateTime()),
                TimeIntervalQuantities = MapProfileResponsesToDeliveryStartTimes(profileResponses, timezone),
                TraderName = tradeDataObject.InternalTrader?.Contact
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

        private string MapCommodityToMarket(CommodityDataObject commodityDataObject)
        {
            return commodityDataObject?.Country;
        }

        private string MapCommodityToCommodity(string commodity)
        {
            var mappingTo = mappingManager.GetMappingTo("EFET_Commodity", commodity);
            return string.IsNullOrEmpty(mappingTo)
                ? throw new DataException("Commodity mapping error")
                : mappingTo;
        }

        private string MapContractTypeToTransactionType(string contractType)
        {
            var mappingTo = mappingManager.GetMappingTo("EFET_TransactionType", contractType);
            return string.IsNullOrEmpty(mappingTo)
                ? throw new DataException("TransactionType mapping error")
                : mappingTo;
        }

        private string MapEicLei(string eic, string lei, string label, bool mandatory = true)
        {
            return string.IsNullOrEmpty(eic)
                ? string.IsNullOrEmpty(lei)
                    ? mandatory
                        ? throw new DataException($"{label} could not be determined")
                        : null
                    : eic
                : lei;
        }

        private bool? MapInternalToIntragroup(bool? buyer, bool? seller)
        {
            return buyer.HasValue && buyer.Value && seller.HasValue && seller.Value
                ? true
                : null;
        }

        private string MapShapeDescriptionToLoadType(string shapeDescription, string defaultValue)
        {
            if (string.IsNullOrEmpty(shapeDescription))
            {
                return defaultValue;
            }

            var mappingTo = mappingManager.GetMappingTo("EFET_LoadType", shapeDescription);
            return string.IsNullOrEmpty(mappingTo)
                ? defaultValue
                : mappingTo;
        }

        private string MapContractAgreementToAgreement(string agreementType)
        {
            // TODO
            var mappingTo = mappingManager.GetMappingTo("EFET_Agreement", agreementType);
            return string.IsNullOrEmpty(mappingTo)
                ? throw new DataException("Agreement mapping error")
                : mappingTo;
        }

        private string MapEnergyUnitToVolumeUnit(string energyUnit)
        {
            var mappingTo = mappingManager.GetMappingTo("EFET_EnergyUnit", energyUnit);
            return string.IsNullOrEmpty(mappingTo)
                ? throw new DataException("TotalVolumeUnit mapping error")
                : mappingTo;
        }

        private string MapQuantityUnitToCapacityUnit(string quantityUnit)
        {
            var mappingTo = mappingManager.GetMappingTo("EFET_CapacityUnit", quantityUnit);
            return string.IsNullOrEmpty(mappingTo)
                ? throw new DataException("CapacityUnit mapping error")
                : mappingTo;
        }

        private PriceUnit MapPriceUnit(PriceUnitDataObject priceUnit)
        {
            return new PriceUnit
            {
                Currency = priceUnit?.PriceUnit,
                UseFractionalUnit = priceUnit?.CurrencyExponent != null && priceUnit.CurrencyExponent != 0,
                CapacityUnit = MapPerEnergyUnitToCapacityUnit(priceUnit?.PerEnergyUnit?.EnergyUnit)
            };
        }

        private string MapPerEnergyUnitToCapacityUnit(string energyUnit)
        {
            var mappingTo = mappingManager.GetMappingTo("EFET_EnergyUnit", energyUnit);
            return string.IsNullOrEmpty(mappingTo)
                ? throw new DataException("PriceUnit.CapacityUnit mapping error")
                : mappingTo;
        }

        private IEnumerable<TimeIntervalQuantity> MapProfileResponsesToDeliveryStartTimes(IEnumerable<ProfileResponse> profileResponses, DateTimeZone dateTimeZone)
        {
            return profileResponses
                .SelectMany(p => p.PriceProfile.Zip(p.VolumeProfile, (price, volume) => (price, volume)))
                .Select(pv => new TimeIntervalQuantity
                {
                    DeliveryStartTimestamp = EquiasDateTimeHelper.FormatDateTimeWithOffset(pv.volume.UtcStartDateTime, dateTimeZone),
                    DeliveryEndTimestamp = EquiasDateTimeHelper.FormatDateTimeWithOffset(pv.volume.UtcEndDateTime, dateTimeZone),
                    Price = pv.price.Value,
                    ContractCapacity = pv.volume.Value
                });
        }
    }
}
