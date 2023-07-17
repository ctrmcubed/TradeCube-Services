using Equias.Constants;
using Equias.Helpers;
using Equias.Models.BackOfficeServices;
using NodaTime;
using Shared.DataObjects;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Messages;
using Shared.Services;
using System;
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

        public EquiasMappingService(IMappingService mappingService, IPartyService partyService)
        {
            this.mappingService = mappingService;
            this.partyService = partyService;
        }

        public async Task<IEnumerable<MappingDataObject>> GetMappingsAsync(string apiJwtToken)
        {
            return (await mappingService.GetMappingsViaJwtAsync(apiJwtToken))?.Data;
        }

        public async Task<PhysicalTrade> MapTrade(TradeDataObject tradeDataObject, TradeSummaryResponse tradeSummaryResponse, IEnumerable<CashflowResponse> cashflowResponses,
            IEnumerable<ProfileResponse> profileResponses, MappingHelper mappingHelper, string apiJwtToken)
        {
            if (tradeDataObject is null)
            {
                throw new DataException("Trade is null");
            }

            if (tradeDataObject.Product?.Commodity?.Timezone is null)
            {
                throw new DataException("Trade's timezone is null");
            }

            if (cashflowResponses is null)
            {
                throw new DataException("No cashflow data");
            }

            if (profileResponses is null)
            {
                throw new DataException("No profile data");
            }

            var timezone = DateTimeHelper.GetDateTimeZone(tradeDataObject.Product?.Commodity?.Timezone);
            var cashflows = cashflowResponses.ToList();
            var commodity = MapCommodityToCommodity(tradeDataObject.Product?.Commodity?.Commodity, mappingHelper);

            var buyerEic = await MapEic(tradeDataObject.Buyer, "Buyer party", apiJwtToken);
            var sellerEic = await MapEic(tradeDataObject.Seller, "Seller party", apiJwtToken);
            var beneficiaryId = await MapEic(tradeDataObject.Beneficiary, "Beneficiary party", apiJwtToken, buyerEic);

            return new PhysicalTrade
            {
                ActionType = tradeDataObject.EboActionType(),
                TradeId = TestMapTradeReferenceToTradeId(tradeDataObject.TradeReference, tradeDataObject.TradeLeg),
                Uti = tradeDataObject.UTI,
                ProcessInformation = new ProcessInformation
                {
                    ReportingOnBehalfOf = false,
                    EmirReportMode = CmsReportType,
                    RemitReportMode = CmsReportType
                },
                Market = MapCommodityToMarket(tradeDataObject.Product?.Commodity),
                Commodity = commodity,
                TransactionType = MapContractTypeToTransactionType(tradeDataObject.Product?.ContractType, mappingHelper),
                DeliveryPointArea = tradeDataObject.Product?.Commodity?.DeliveryArea?.Eic,
                BuyerParty = buyerEic,
                SellerParty = sellerEic,
                BeneficiaryId = beneficiaryId,
                Intragroup = MapInternalToIntragroup(tradeDataObject.Buyer?.Internal, tradeDataObject.Seller?.Internal),
                LoadType = MapShapeDescriptionToLoadType(tradeDataObject.Product?.ShapeDescription, "Custom", mappingHelper),
                Agreement = MapContractAgreementToAgreement(tradeDataObject.Contract?.AgreementType?.AgreementType, tradeDataObject.Product?.Commodity?.Commodity, mappingHelper),
                TotalVolume = AbsoluteValue(tradeSummaryResponse?.TotalVolume),
                TotalVolumeUnit = MapEnergyUnitToVolumeUnit(tradeDataObject.Quantity?.QuantityUnit?.EnergyUnit?.EnergyUnit, mappingHelper),
                TradeExecutionTimestamp = EquiasDateTimeHelper.FormatDateTimeWithOffset(tradeDataObject.TradeDateTime, timezone),
                CapacityUnit = MapQuantityUnitToCapacityUnit(tradeDataObject.Quantity?.QuantityUnit?.QuantityUnit, mappingHelper),
                PriceUnit = MapPriceUnit(tradeDataObject.Price?.PriceUnit, mappingHelper),
                TotalContractValue = AbsoluteValue(tradeSummaryResponse?.TotalValue),
                SettlementCurrency = tradeSummaryResponse?.TotalValueCurrency,
                SettlementDates = cashflows.Any()
                    ? cashflows.SelectMany(d => d.Cashflows.Select(c => c.SettlementDate.ToIso8601DateTime()))
                    : null,
                TimeIntervalQuantities = MapProfileResponsesToDeliveryStartTimes(tradeDataObject.Quantity?.Quantity, profileResponses, timezone, tradeDataObject.Price?.PriceUnit?.CurrencyExponent),
                TraderName = tradeDataObject.InternalTrader?.ContactLongName,
                HubCodificationInformation = commodity == EquiasConstants.CommodityGas
                    ? await MapHubCodificationInformation(tradeDataObject.Buyer, tradeDataObject.Seller, apiJwtToken)
                    : null,
                Agents = commodity == EquiasConstants.CommodityPower
                    ? new List<Agent>
                    {
                        new()
                        {
                            AgentName = tradeDataObject.Extension?.BuyerEnergyAccount,
                            AgentType = EquiasConstants.AgentTypeEcvna,
                            Ecvna = new Ecvna
                            {
                                BscPartyId = await MapBscParty(tradeDataObject.Extension?.EcvnAgentParty?.Extension?.BscParty, apiJwtToken),
                                BuyerEnergyAccount = tradeDataObject.Extension?.BuyerEnergyAccount,
                                SellerEnergyAccount = tradeDataObject.Extension?.SellerEnergyAccount,
                                BuyerId =  await MapBuyerSellerId(tradeDataObject.Buyer,  apiJwtToken),
                                SellerId =  await MapBuyerSellerId(tradeDataObject.Seller,  apiJwtToken),
                                NotificationAgent =  await MapNotificationAgent(tradeDataObject.Extension?.EcvnAgentParty, apiJwtToken),
                                TransmissionChargeIdentification =  MapSchedule5(tradeDataObject.Extension?.Schedule5, mappingHelper)
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

        private static decimal? AbsoluteValue(decimal? value) =>
            value.HasValue
                ? Math.Abs(value.Value)
                : null;

        private static decimal AbsoluteValue(decimal value) =>
            Math.Abs(value);

        private string MapCommodityToMarket(CommodityDataObject commodityDataObject)
        {
            return commodityDataObject?.Country;
        }

        private string MapCommodityToCommodity(string commodity, MappingHelper mappingHelper)
        {
            var mappingTo = mappingHelper.GetMappingTo("EFET_Commodity", commodity);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? commodity
                : mappingTo;
        }

        private string MapContractTypeToTransactionType(string contractType, MappingHelper mappingHelper)
        {
            var mappingTo = mappingHelper.GetMappingTo("EFET_TransactionType", contractType);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? contractType
                : mappingTo;
        }

        private async Task<string> MapEic(PartyDataObject party, string label, string apiJwtToken, string defaultValue = null)
        {
            async Task<PartyDataObject> GetPartyAsync(string pty)
            {
                return (await partyService.GetPartyAsync(pty, apiJwtToken))?.Data?.SingleOrDefault();
            }

            if (!string.IsNullOrWhiteSpace(party?.Eic?.Eic))
            {
                return party.Eic?.Eic;
            }

            if (string.IsNullOrWhiteSpace(party?.Party))
            {
                return string.IsNullOrWhiteSpace(defaultValue)
                    ? throw new DataException($"The {label} does not have an EIC")
                    : defaultValue;
            }

            var partyDataObject = await GetPartyAsync(party.Party);
            if (!string.IsNullOrWhiteSpace(partyDataObject?.Eic?.Eic))
            {
                return partyDataObject.Eic?.Eic;
            }

            return string.IsNullOrWhiteSpace(defaultValue)
                ? throw new DataException($"The {label} does not have an EIC")
                : defaultValue;
        }

        private bool? MapInternalToIntragroup(bool? buyer, bool? seller)
        {
            return buyer.HasValue && buyer.Value && seller.HasValue && seller.Value
                ? true
                : null;
        }

        private string MapShapeDescriptionToLoadType(string shapeDescription, string defaultValue, MappingHelper mappingHelper)
        {
            if (string.IsNullOrWhiteSpace(shapeDescription))
            {
                return defaultValue;
            }

            var mappingTo = mappingHelper.GetMappingTo("EFET_LoadType", shapeDescription);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? defaultValue
                : mappingTo;
        }

        private string MapContractAgreementToAgreement(string agreementType, string commodity, MappingHelper mappingHelper)
        {
            if (!string.IsNullOrWhiteSpace(agreementType))
            {
                return agreementType;
            }

            var mappingTo = mappingHelper.GetMappingTo("EFET_Agreement", commodity);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? agreementType
                : mappingTo;
        }

        private string MapEnergyUnitToVolumeUnit(string energyUnit, MappingHelper mappingHelper)
        {
            var mappingTo = mappingHelper.GetMappingTo("EFET_EnergyUnit", energyUnit);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? energyUnit
                : mappingTo;
        }

        private string MapQuantityUnitToCapacityUnit(string quantityUnit, MappingHelper mappingHelper)
        {
            var mappingTo = mappingHelper.GetMappingTo("EFET_CapacityUnit", quantityUnit);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? quantityUnit
                : mappingTo;
        }

        private PriceUnit MapPriceUnit(PriceUnitDataObject priceUnit, MappingHelper mappingHelper)
        {
            return new PriceUnit
            {
                Currency = priceUnit?.Currency,
                UseFractionalUnit = priceUnit?.CurrencyExponent is not null && priceUnit.CurrencyExponent != 0,
                CapacityUnit = MapPerEnergyUnitToCapacityUnit(priceUnit?.PerQuantityUnit?.QuantityUnit, mappingHelper)
            };
        }

        private string MapPerEnergyUnitToCapacityUnit(string quantityUnit, MappingHelper mappingHelper)
        {
            var mappingTo = mappingHelper.GetMappingTo("EFET_PriceUnit_CapacityUnit", quantityUnit);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? quantityUnit
                : mappingTo;
        }

        private string MapSchedule5(string schedule5, MappingHelper mappingHelper)
        {
            var mappingTo = mappingHelper.GetMappingTo("EFET_Schedule5", schedule5);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? schedule5
                : mappingTo;
        }

        private static IEnumerable<TimeIntervalQuantity> MapProfileResponsesToDeliveryStartTimes(decimal? quantity, IEnumerable<ProfileResponse> profileResponses, DateTimeZone dateTimeZone, int? currencyExponent)
        {
            static IEnumerable<(DateTime utcStart, DateTime utcEnd, decimal volume, decimal price)> Zip(IEnumerable<ProfileBase> volumes, IEnumerable<ProfileBase> prices)
            {
                var priceList = prices.ToList();
                var firstPrice = priceList.FirstOrDefault();
                var priceDict = priceList.ToDictionary(k => k.UtcStartDateTime, v => v);

                foreach (var volume in volumes)
                {
                    if (priceDict.ContainsKey(volume.UtcStartDateTime))
                    {
                        yield return (volume.UtcStartDateTime, volume.UtcEndDateTime, volume.Value, priceDict[volume.UtcStartDateTime].Value);
                    }
                    else
                    {
                        yield return (volume.UtcStartDateTime, volume.UtcEndDateTime, volume.Value, firstPrice?.Value ?? 0);
                    }
                }
            }

            return profileResponses
                .SelectMany(p => Zip(p.VolumeProfile, p.PriceProfile))
                .Where(v => v.volume != 0.0m)
                .Select(pv => new TimeIntervalQuantity
                {
                    DeliveryStartTimestamp = EquiasDateTimeHelper.FormatDateTimeWithOffset(pv.utcStart, dateTimeZone),
                    DeliveryEndTimestamp = EquiasDateTimeHelper.FormatDateTimeWithOffset(pv.utcEnd, dateTimeZone),
                    Price = pv.price * (currencyExponent.HasValue
                        ? (decimal)Math.Pow(10, currencyExponent.Value)
                        : 1.0m),
                    ContractCapacity = quantity.HasValue
                        ? AbsoluteValue(quantity.Value)
                        : 0
                });
        }

        private async Task<HubCodificationInformation> MapHubCodificationInformation(PartyDataObject buyer, PartyDataObject seller, string apiJwtToken)
        {
            async Task<string> HubCode(PartyDataObject party)
            {
                return party?.Extension?.UkGasShipper?.ShipperCode ??
                       (await partyService.GetPartyAsync(party?.Party, apiJwtToken))?.Data?.SingleOrDefault()?.Extension?.UkGasShipper?.ShipperCode;
            }

            return new HubCodificationInformation
            {
                BuyerHubCode = await HubCode(buyer),
                SellerHubCode = await HubCode(seller)
            };
        }

        private async Task<string> MapBscParty(UkBscPartyDataObject ukBscPartyDataObject, string apiJwtToken)
        {
            async Task<string> Id(UkBscPartyDataObject bsc)
            {
                return bsc?.BscPartyId ??
                       (await partyService.GetPartyAsync(bsc?.BscPartyId, apiJwtToken))?.Data?.SingleOrDefault()?.Extension?.BscParty?.BscPartyId;
            }

            return ukBscPartyDataObject is null
                ? throw new DataException("The ECVN Agent has no BSC Party")
                : await Id(ukBscPartyDataObject);
        }

        private async Task<string> MapBuyerSellerId(PartyDataObject party, string apiJwtToken)
        {
            async Task<string> Id(PartyDataObject pty)
            {
                return pty?.Extension?.BscParty?.BscPartyId ??
                       (await partyService.GetPartyAsync(pty?.Party, apiJwtToken))?.Data?.SingleOrDefault()?.Extension?.BscParty?.BscPartyId;
            }

            return party is null
                ? throw new DataException("The trade has no Buyer or Seller")
                : await Id(party);
        }

        private async Task<string> MapNotificationAgent(PartyDataObject party, string apiJwtToken)
        {
            async Task<string> Eic(PartyDataObject pty)
            {
                return pty?.Eic?.Eic ??
                       (await partyService.GetPartyAsync(pty?.Party, apiJwtToken))?.Data?.SingleOrDefault()?.Eic?.Eic;
            }

            return party is null
                ? throw new DataException("The trade has no ECVN Agent")
                : await Eic(party);
        }
    }
}
