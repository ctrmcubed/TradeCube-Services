using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Extensions;
using TradeCube_Services.Helpers;
using TradeCube_Services.Messages;

namespace TradeCube_Services.Services.ThirdParty.ETRMServices
{
    public class M7TradeService : IM7TradeService
    {
        private readonly IMappingService mappingService;
        private readonly IFingerprintService fingerprintService;
        private readonly IPartyService partyService;
        private readonly ILogger<M7TradeService> logger;

        public M7TradeService(IMappingService mappingService, IFingerprintService fingerprintService, IPartyService partyService, ILogger<M7TradeService> logger)
        {
            this.mappingService = mappingService;
            this.fingerprintService = fingerprintService;
            this.partyService = partyService;
            this.logger = logger;
        }

        public async Task<TradeDataObject> ConvertTradeAsync(XElement m7Trade, string apiKey)
        {
            async Task<MappingDataObject> MapDeliveryAreaToCommodityAsync(string deliveryAreaId)
            {
                var apiResponseWrapper = await mappingService.GetMappingAsync("M7_Commodity", deliveryAreaId, apiKey);
                return apiResponseWrapper.Data.Any()
                    ? apiResponseWrapper.Data.FirstOrDefault()
                    : throw new DataException($"Could not map Delivery Area '{deliveryAreaId}' to Commodity");
            }

            async Task<MappingDataObject> MapExchangeIdToPartyAsync(string exchangeId)
            {
                var apiResponseWrapper = await mappingService.GetMappingAsync("M7_Party", exchangeId, apiKey);
                return apiResponseWrapper.Data.Any()
                    ? apiResponseWrapper.Data.FirstOrDefault()
                    : throw new DataException($"Could not map Exchange Id '{exchangeId}' to Party");
            }

            async Task<FingerprintResponse> MapCommodityToProductAsync(string commodity, DateTime start, DateTime end)
            {
                var fingerprintRequest = new FingerprintRequest
                {
                    Commodity = commodity,
                    ProfileDefinition = new List<ProfileDefinitionType>
                    {
                        new ProfileDefinitionType
                        {
                            UtcStartDateTime = start,
                            UtcEndDateTime = end
                        }
                    }
                };

                var apiResponseWrapper = await fingerprintService.FingerprintAsync(apiKey, fingerprintRequest);
                return apiResponseWrapper.Data.Any()
                    ? apiResponseWrapper.Data.FirstOrDefault()
                    : throw new DataException($"Could not map Commodity '{commodity}' to Product");
            }

            async Task<PartyDataObject> MapCounterpartyToPartyAsync(string party)
            {
                var apiResponseWrapper = await partyService.GetPartyAsync(party, apiKey);
                return apiResponseWrapper.Data.Any()
                    ? apiResponseWrapper.Data.FirstOrDefault()
                    : throw new DataException($"Could not map Counterparty '{party}' to Party");
            }

            try
            {
                var tradeId = m7Trade?.Attribute("tradeId")?.Value;
                var contractId = m7Trade?.Attribute("contractId")?.Value;
                var execTimeUtc = m7Trade?.Attribute("execTimeUTC")?.Value;
                var side = m7Trade?.Attribute("side")?.Value;
                var deliveryAreaId = m7Trade?.Attribute("dlvryAreaId")?.Value;
                var qty = m7Trade?.Attribute("qty")?.Value;
                var px = m7Trade?.Attribute("px")?.Value;
                var deliveryStartUtc = m7Trade?.Element("Contract")?.Attribute("dlvryStartUTC")?.Value;
                var deliveryEndUTc = m7Trade?.Element("Contract")?.Attribute("dlvryEndUTC")?.Value;
                var exchangeId = m7Trade?.Element("Product")?.Attribute("exchangeId")?.Value;

                // Type conversion
                var tradeDateTime = execTimeUtc.FromIso8601DateTime();
                var deliveryStart = deliveryStartUtc.FromIso8601DateTime();
                var deliveryEnd = deliveryEndUTc.FromIso8601DateTime();
                var quantity = StringHelper.SafeParse(qty);
                var price = StringHelper.SafeParse(px);

                // Mappings
                var tradeStatus = "Live";
                var quantityType = "Fixed";
                var priceType = "Fixed";
                var buySell = MapSideToBuySell(side);

                var commodityTask = MapDeliveryAreaToCommodityAsync(deliveryAreaId);
                var counterpartyTask = MapExchangeIdToPartyAsync(exchangeId);

                var commodity = await commodityTask;
                var counterparty = await counterpartyTask;

                var fingerprintTask = MapCommodityToProductAsync(commodity?.MappingTo, deliveryStart, deliveryEnd);
                var partyTask = MapCounterpartyToPartyAsync(counterparty?.MappingTo);

                var tradingBook = commodity?.MappingTo;
                var fingerprint = await fingerprintTask;
                var party = await partyTask;

                var trade = new TradeDataObject
                {
                    TradeReference = tradeId,
                    TradeLeg = 1,
                    TradeDateTime = tradeDateTime,
                    TradeStatus = tradeStatus,
                    BuySell = buySell,
                    TradingBook = new TradingBookDataObject { TradingBook = tradingBook },
                    Product = fingerprint?.ProductDataObject,
                    Contract = new ContractDataObject { ContractReference = contractId },
                    Counterparty = party,
                    Quantity = new QuantityDataObject
                    {
                        Quantity = quantity,
                        QuantityType = quantityType,
                        QuantityUnit = fingerprint?.ProductDataObject?.QuantityUnit
                    },
                    Price = new PriceDataObject
                    {
                        Price = price,
                        PriceType = priceType,
                        PriceUnit = fingerprint?.ProductDataObject.PriceUnit
                    }
                };

                return trade;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error creating trade from M7 ({ex.Message})");
                throw;
            }
        }

        private static string MapSideToBuySell(string side)
        {
            return side.ToLower() switch
            {
                "buy" => "Buy",
                "sell" => "Sell",
                "swap" => "Swap",
                _ => throw new DataException($"Could not map side '{side}' to buy/sell/swap")
            };
        }
    }
}
