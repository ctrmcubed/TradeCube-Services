using Fidectus.Constants;
using Fidectus.Helpers;
using Fidectus.Models;
using NodaTime;
using Shared.DataObjects;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Messages;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Fidectus.Services
{
    public class FidectusMappingService : IFidectusMappingService
    {
        private readonly IMappingService mappingService;
        private readonly IPartyService partyService;

        public FidectusMappingService(IMappingService mappingService, IPartyService partyService)
        {
            this.mappingService = mappingService;
            this.partyService = partyService;
        }

        public string MapTradeReferenceToTradeId(string tradeReference, int tradeLeg)
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

        public async Task<IEnumerable<MappingDataObject>> GetMappingsAsync(string apiJwtToken)
        {
            return (await mappingService.GetMappingsViaJwtAsync(apiJwtToken))?.Data;
        }

        public async Task<TradeConfirmation> MapConfirmation(TradeDataObject tradeDataObject, TradeSummaryResponse tradeSummaryResponse,
            IEnumerable<ProfileResponse> profileResponses, ConfigurationHelper configurationHelper, string apiJwtToken)
        {
            if (tradeDataObject == null)
            {
                throw new DataException("Trade is null");
            }

            if (tradeDataObject.Product?.Commodity?.Timezone == null)
            {
                throw new DataException("Trade's timezone is null");
            }

            if (profileResponses == null)
            {
                throw new DataException("No profile data");
            }


            var timezone = DateTimeHelper.GetTimeZone(tradeDataObject.Product?.Commodity?.Timezone);

            var senderId = await MapSenderId(tradeDataObject, apiJwtToken);
            var receiverId = await MapReceiverId(tradeDataObject, apiJwtToken);
            var buyerEic = await MapEic(tradeDataObject.Buyer, "Buyer party", apiJwtToken);
            var sellerEic = await MapEic(tradeDataObject.Seller, "Seller party", apiJwtToken);

            var mappingHelper = configurationHelper.MappingHelper;
            var settingHelper = configurationHelper.SettingHelper;

            return new TradeConfirmation
            {
                DocumentId = MapDocumentId(tradeDataObject, senderId),
                DocumentUsage = settingHelper.GetSetting("FidectusConfirmationUsage", "Live"),
                SenderId = senderId,
                ReceiverId = receiverId,
                ReceiverRole = "Trader",
                DocumentVersion = 1,
                Market = tradeDataObject.Product?.Commodity?.Country,
                Commodity = MapCommodityToCommodity(tradeDataObject.Product?.Commodity?.Commodity, mappingHelper),
                TransactionType = MapContractTypeToTransactionType(tradeDataObject.Product?.ContractType, mappingHelper),
                DeliveryPointArea = tradeDataObject.Product?.Commodity?.DeliveryArea?.Eic,
                BuyerParty = buyerEic,
                SellerParty = sellerEic,
                LoadType = MapShapeDescriptionToLoadType(tradeDataObject.Product?.ShapeDescription, "Custom", mappingHelper),
                Agreement = MapContractAgreementToAgreement(tradeDataObject.Contract?.AgreementType?.AgreementType, tradeDataObject.Product?.Commodity?.Commodity, mappingHelper),
                Currency = MapPriceUnitToCurrency(tradeDataObject.Price?.PriceUnit),
                TotalVolume = AbsoluteValue(tradeSummaryResponse?.TotalVolume),
                TotalVolumeUnit = MapEnergyUnitToVolumeUnit(tradeDataObject.Quantity?.QuantityUnit?.EnergyUnit?.EnergyUnit, mappingHelper),
                TradeDate = tradeDataObject.TradeDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                CapacityUnit = MapQuantityUnitToCapacityUnit(tradeDataObject.Quantity?.QuantityUnit?.QuantityUnit, mappingHelper),
                PriceUnit = MapPriceUnitToPriceUnit(tradeDataObject.Price?.PriceUnit, mappingHelper),
                TotalContractValue = AbsoluteValue(tradeSummaryResponse?.TotalValue),
                TimeIntervalQuantities = MapProfileResponsesToDeliveryStartTimes(tradeDataObject.Quantity?.Quantity, profileResponses, timezone, tradeDataObject.Price?.PriceUnit?.CurrencyExponent),
                TraderName = tradeDataObject.InternalTrader?.ContactLongName,
                HubCodificationInformation = MapCommodityToCommodity(tradeDataObject.Product?.Commodity?.Commodity, mappingHelper) == FidectusConstants.CommodityGas
                    ? await MapHubCodificationInformation(tradeDataObject.Buyer, tradeDataObject.Seller, apiJwtToken)
                    : null,
                AccountAndChargeInformation = MapAccountAndChargeInformation(tradeDataObject),
                Agents = MapCommodityToCommodity(tradeDataObject.Product?.Commodity?.Commodity, mappingHelper) == FidectusConstants.CommodityPower
                    ? new List<Agent>
                    {
                        new()
                        {
                            AgentName = tradeDataObject.Extension?.EcvnAgentParty?.Extension?.BscParty?.BscPartyLongName,
                            AgentType = FidectusConstants.AgentTypeEcvna,
                            Ecvna = new Ecvna
                            {
                                BscPartyId = await MapBscParty(tradeDataObject.Extension?.EcvnAgentParty?.Extension?.BscParty, apiJwtToken),
                                BuyerEnergyAccount = tradeDataObject.Extension?.BuyerEnergyAccount,
                                SellerEnergyAccount = tradeDataObject.Extension?.SellerEnergyAccount,
                                BuyerId = await MapBuyerSellerId(tradeDataObject.Buyer, apiJwtToken),
                                SellerId = await MapBuyerSellerId(tradeDataObject.Seller, apiJwtToken),
                            }
                        }
                    }
                    : null
            };
        }

        private async Task<string> MapSenderId(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            async Task<string> Party(TradeDataObject trade, string token)
            {
                var party = await partyService.GetPartyAsync(trade.Counterparty?.Party, token);
                var singleOrDefault = party?.Data?.SingleOrDefault();

                if (singleOrDefault is null)
                {
                    throw new DataException("The Internal party does not have an EIC or LEI");
                }

                if (!string.IsNullOrEmpty(singleOrDefault.Eic?.Eic))
                {
                    return singleOrDefault.Eic?.Eic;
                }

                return string.IsNullOrEmpty(singleOrDefault.Lei?.Lei)
                    ? throw new DataException("The Internal party does not have an EIC or LEI")
                    : singleOrDefault.Lei?.Lei;
            }

            async Task<string> Lei(TradeDataObject trade, string token)
            {
                return string.IsNullOrWhiteSpace(tradeDataObject.InternalParty?.Lei?.Lei)
                    ? await Party(trade, token)
                    : tradeDataObject.InternalParty?.Lei?.Lei;
            }

            return string.IsNullOrWhiteSpace(tradeDataObject.InternalParty?.Eic?.Eic)
                ? await Lei(tradeDataObject, apiJwtToken)
                : tradeDataObject.InternalParty?.Eic?.Eic;
        }

        private async Task<string> MapReceiverId(TradeDataObject tradeDataObject, string apiJwtToken)
        {
            async Task<string> Party(TradeDataObject trade, string token)
            {
                var party = await partyService.GetPartyAsync(trade.Counterparty?.Party, token);
                var singleOrDefault = party?.Data?.SingleOrDefault();

                if (singleOrDefault is null)
                {
                    throw new DataException("The Counterparty does not have an EIC or LEI");
                }

                if (!string.IsNullOrEmpty(singleOrDefault.Eic?.Eic))
                {
                    return singleOrDefault.Eic?.Eic;
                }

                return string.IsNullOrEmpty(singleOrDefault.Lei?.Lei)
                    ? throw new DataException("The Counterparty party does not have an EIC or LEI")
                    : singleOrDefault.Lei?.Lei;
            }

            async Task<string> Lei(TradeDataObject trade, string token)
            {
                return string.IsNullOrWhiteSpace(tradeDataObject.Counterparty?.Lei?.Lei)
                    ? await Party(trade, token)
                    : tradeDataObject.Counterparty?.Lei?.Lei;
            }

            return string.IsNullOrWhiteSpace(tradeDataObject.Counterparty?.Eic?.Eic)
                ? await Lei(tradeDataObject, apiJwtToken)
                : tradeDataObject.Counterparty?.Eic?.Eic;
        }

        private string MapDocumentId(TradeDataObject tradeDataObject, string senderId)
        {
            var tradeId = MapTradeReferenceToTradeId(tradeDataObject.TradeReference, tradeDataObject.TradeLeg);
            var tradeDateTime = tradeDataObject.TradeDateTime.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            return $"{FidectusConstants.ConfirmationDocument}_{tradeDateTime}_{tradeId}@{senderId}";
        }

        private static decimal? AbsoluteValue(decimal? value) =>
            value.HasValue
                ? Math.Abs(value.Value)
                : null;

        private static decimal AbsoluteValue(decimal value) =>
            Math.Abs(value);

        private static string MapCommodityToCommodity(string commodity, MappingHelper mappingHelper)
        {
            var mappingTo = mappingHelper.GetMappingTo("EFET_Commodity", commodity);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? commodity
                : mappingTo;
        }

        private static string MapContractTypeToTransactionType(string contractType, MappingHelper mappingHelper)
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

        private static string MapShapeDescriptionToLoadType(string shapeDescription, string defaultValue, MappingHelper mappingHelper)
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

        private static string MapContractAgreementToAgreement(string agreementType, string commodity, MappingHelper mappingHelper)
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

        private static Currency MapPriceUnitToCurrency(PriceUnitDataObject priceUnit)
        {
            return new()
            {
                CurrencyCodeType = priceUnit?.Currency,
                UseFractionUnit = priceUnit?.CurrencyExponent is not null
            };
        }

        private static string MapEnergyUnitToVolumeUnit(string energyUnit, MappingHelper mappingHelper)
        {
            var mappingTo = mappingHelper.GetMappingTo("EFET_EnergyUnit", energyUnit);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? energyUnit
                : mappingTo;
        }

        private static string MapQuantityUnitToCapacityUnit(string quantityUnit, MappingHelper mappingHelper)
        {
            var mappingTo = mappingHelper.GetMappingTo("EFET_CapacityUnit", quantityUnit);
            return string.IsNullOrWhiteSpace(mappingTo)
                ? quantityUnit
                : mappingTo;
        }

        private static PriceUnit MapPriceUnitToPriceUnit(PriceUnitDataObject priceUnit, MappingHelper mappingHelper)
        {
            return new()
            {
                Currency = new Currency
                {
                    CurrencyCodeType = priceUnit?.Currency,
                    UseFractionUnit = priceUnit?.CurrencyExponent is not null
                },
                CapacityUnit = mappingHelper.GetMappingTo("EFET_PriceUnit_CapacityUnit", priceUnit?.PerQuantityUnit?.QuantityUnit)
            };
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
                    DeliveryStartTimestamp = FidectusDateTimeHelper.FormatDateTimeWithOffset(pv.utcStart, dateTimeZone),
                    DeliveryEndTimestamp = FidectusDateTimeHelper.FormatDateTimeWithOffset(pv.utcEnd, dateTimeZone),
                    Price = pv.price * (currencyExponent.HasValue
                        ? (decimal)Math.Pow(10, currencyExponent.Value)
                        : 1.0m),
                    ContractCapacity = quantity.HasValue
                        ? AbsoluteValue(quantity.Value)
                        : 0
                });
        }

        private static AccountAndChargeInformation MapAccountAndChargeInformation(TradeDataObject tradeDataObject)
        {
            return new AccountAndChargeInformation
            {
                BuyerEnergyAccountIdentification = tradeDataObject?.Extension?.BuyerEnergyAccount,
                SellerEnergyAccountIdentification = tradeDataObject?.Extension?.SellerEnergyAccount,
                TransmissionChargeIdentification = tradeDataObject?.Extension?.Schedule5,
                NotificationAgent = tradeDataObject?.Extension?.EcvnAgentParty?.Eic?.Eic
            };
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

            return ukBscPartyDataObject == null
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

            return party == null
                ? throw new DataException("The trade has no Buyer or Seller")
                : await Id(party);
        }
    }
}
