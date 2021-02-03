using Equias.Constants;
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
        private readonly IPartyService partyService;
        private const string CmsReportType = "CmsReport";
        private MappingManager mappingManager;

        public EquiasMappingService(IMappingService mappingService, IPartyService partyService)
        {
            this.mappingService = mappingService;
            this.partyService = partyService;
        }

        public EquiasMappingService SetMappingManager(MappingManager mapping)
        {
            mappingManager = mapping;
            return this;
        }

        public async Task<IEnumerable<MappingDataObject>> GetMappingsAsync(string apiJwtToken)
        {
            return (await mappingService.GetMappingsViaJwtAsync(apiJwtToken))?.Data;
        }

        public async Task<PhysicalTrade> MapTrade(TradeDataObject tradeDataObject, TradeSummaryResponse tradeSummaryResponse, IEnumerable<CashflowResponse> cashflowResponses,
            IEnumerable<ProfileResponse> profileResponses, string apiJwtToken)
        {
            if (tradeDataObject == null)
            {
                throw new DataException("Trade is null");
            }

            if (tradeDataObject.Product?.Commodity?.Timezone == null)
            {
                throw new DataException("Trade's timezone is null");
            }

            if (cashflowResponses == null)
            {
                throw new DataException("No cashflow data");
            }

            if (profileResponses == null)
            {
                throw new DataException("No profile data");
            }

            var timezone = DateTimeHelper.GetTimeZone(tradeDataObject.Product?.Commodity?.Timezone);
            var cashflows = cashflowResponses.ToList();
            var commodity = MapCommodityToCommodity(tradeDataObject.Product?.Commodity?.Commodity);

            return new PhysicalTrade
            {
                TradeId = TestMapTradeReferenceToTradeId(tradeDataObject.TradeReference, tradeDataObject.TradeLeg),
                Uti = tradeDataObject.Uti,
                ProcessInformation = new ProcessInformation
                {
                    ReportingOnBehalfOf = true,
                    EmirReportMode = CmsReportType,
                    RemitReportMode = CmsReportType
                },
                Market = MapCommodityToMarket(tradeDataObject.Product?.Commodity),
                Commodity = commodity,
                TransactionType = MapContractTypeToTransactionType(tradeDataObject.Product?.ContractType),
                DeliveryPointArea = tradeDataObject.Product?.Commodity?.DeliveryArea?.Eic,
                BuyerParty = await MapEic(tradeDataObject.Buyer?.Party, tradeDataObject.Buyer?.Eic?.Eic, "Buyer party", apiJwtToken),
                SellerParty = await MapEic(tradeDataObject.Seller?.Party, tradeDataObject.Seller?.Eic?.Eic, "Seller party", apiJwtToken),
                BeneficiaryId = await MapEic(tradeDataObject.Beneficiary?.Party, tradeDataObject.Beneficiary?.Eic?.Eic, "Beneficiary party", apiJwtToken),
                Intragroup = MapInternalToIntragroup(tradeDataObject.Buyer?.Internal, tradeDataObject.Seller?.Internal),
                LoadType = MapShapeDescriptionToLoadType(tradeDataObject.Product?.ShapeDescription, "Custom"),
                Agreement = MapContractAgreementToAgreement(tradeDataObject.Contract?.AgreementType?.AgreementType, tradeDataObject.Product?.Commodity?.Commodity),
                TotalVolume = tradeSummaryResponse?.TotalVolume,
                TotalVolumeUnit = MapEnergyUnitToVolumeUnit(tradeDataObject.Quantity?.QuantityUnit?.EnergyUnit?.EnergyUnit),
                TradeExecutionTimestamp = EquiasDateTimeHelper.FormatDateTimeWithOffset(tradeDataObject.TradeDateTime, timezone),
                CapacityUnit = MapQuantityUnitToCapacityUnit(tradeDataObject.Quantity?.QuantityUnit?.QuantityUnit),
                PriceUnit = MapPriceUnit(tradeDataObject.Price?.PriceUnit),
                TotalContractValue = tradeSummaryResponse?.TotalValue,
                SettlementCurrency = tradeSummaryResponse?.TotalValueCurrency,
                SettlementDates = cashflows.Any()
                    ? cashflows.SelectMany(d => d.Cashflows.Select(c => c.SettlementDate.ToIso8601DateTime()))
                    : null,
                TimeIntervalQuantities = MapProfileResponsesToDeliveryStartTimes(profileResponses, timezone),
                TraderName = tradeDataObject.InternalTrader?.ContactLongName,
                HubCodificationInformation = MapHubCodificationInformation(tradeDataObject.External),
                Agents = commodity == EquiasConstants.CommodityPower
                    ? new List<Agent>
                    {
                        new()
                        {
                            AgentName = tradeDataObject.External?.UkPowerEcvn?.BscPartyId,
                            AgentType = EquiasConstants.AgentTypeEcvna,
                            Ecvna = new Ecvna
                            {
                                BscPartyId = tradeDataObject.External?.UkPowerEcvn?.BscPartyId,
                                BuyerEnergyAccount = tradeDataObject.External?.UkPowerEcvn?.BuyerEnergyAccount,
                                SellerEnergyAccount = tradeDataObject.External?.UkPowerEcvn?.SellerEnergyAccount,
                                BuyerId =  tradeDataObject.External?.UkPowerEcvn?.BuyerId,
                                SellerId =  tradeDataObject.External?.UkPowerEcvn?.SellerId,
                                NotificationAgent =  tradeDataObject.External?.UkPowerEcvn?.NotificationAgent,
                                TransmissionChargeIdentification =  tradeDataObject.External?.UkPowerEcvn?.TransmissionChargeIdentification,
                            }
                        }
                    }
                    : null
            };
        }

        public static string TestMapTradeReferenceToTradeId(string tradeReference, int tradeLeg)
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

        private async Task<string> MapEic(string party, string eic, string label, string apiJwtToken)
        {
            async Task<PartyDataObject> GetPartyAsync(string pty)
            {
                return (await partyService.GetPartyAsync(pty, apiJwtToken))?.Data?.SingleOrDefault();
            }

            if (!string.IsNullOrEmpty(eic))
            {
                return eic;
            }

            if (string.IsNullOrEmpty(party))
            {
                throw new DataException($"The {label} does not have an EIC");
            }

            var partyDataObject = await GetPartyAsync(party);
            if (!string.IsNullOrEmpty(partyDataObject?.Eic?.Eic))
            {
                return partyDataObject.Eic?.Eic;
            }

            throw new DataException($"The {label} does not have an EIC");
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

        private string MapContractAgreementToAgreement(string agreementType, string commodity)
        {
            if (!string.IsNullOrEmpty(agreementType))
            {
                return agreementType;
            }

            var mappingTo = mappingManager.GetMappingTo("EFET_Agreement", commodity);
            return string.IsNullOrEmpty(mappingTo)
                ? throw new DataException($"Agreement mapping error ({commodity})")
                : mappingTo;
        }

        private string MapEnergyUnitToVolumeUnit(string energyUnit)
        {
            var mappingTo = mappingManager.GetMappingTo("EFET_EnergyUnit", energyUnit);
            return string.IsNullOrEmpty(mappingTo)
                ? throw new DataException($"TotalVolumeUnit mapping error ({energyUnit})")
                : mappingTo;
        }

        private string MapQuantityUnitToCapacityUnit(string quantityUnit)
        {
            var mappingTo = mappingManager.GetMappingTo("EFET_CapacityUnit", quantityUnit);
            return string.IsNullOrEmpty(mappingTo)
                ? throw new DataException($"CapacityUnit mapping error ({quantityUnit})")
                : mappingTo;
        }

        private PriceUnit MapPriceUnit(PriceUnitDataObject priceUnit)
        {
            return new PriceUnit
            {
                Currency = priceUnit?.Currency,
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

        private HubCodificationInformation MapHubCodificationInformation(ExternalFieldsType externalFieldsType)
        {
            return string.IsNullOrEmpty(externalFieldsType?.UkGasHub?.BuyerHubCode) && string.IsNullOrEmpty(externalFieldsType?.UkGasHub?.SellerHubCode)
            ? null
            : new HubCodificationInformation
            {
                BuyerHubCode = externalFieldsType.UkGasHub?.BuyerHubCode,
                SellerHubCode = externalFieldsType.UkGasHub?.SellerHubCode
            };
        }
    }
}
