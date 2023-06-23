using System.Data;
using Shared.DataObjects;
using Shared.Messages;
using Shared.Services;
using TradeCube_Services.Models.ThirdParty.ETRMServices;

namespace TradeCube_Services.Services.ThirdParty.ETRMServices
{
    public class M7TradeService : IM7TradeService
    {
        private readonly IMappingService mappingService;
        private readonly IFingerprintService fingerprintService;
        private readonly IM7PartyService im7PartyService;
        private readonly IContactService contactService;
        private readonly IVenueService venueService;
        private readonly ISettingService settingService;
        private readonly ITradingBookService tradingBookService;
        private readonly ILogger<M7TradeService> logger;

        public M7TradeService(IMappingService mappingService, ISettingService settingService, ITradingBookService tradingBookService, IFingerprintService fingerprintService,
            IM7PartyService im7PartyService, IContactService contactService, IVenueService venueService, ILogger<M7TradeService> logger)
        {
            this.mappingService = mappingService;
            this.fingerprintService = fingerprintService;
            this.im7PartyService = im7PartyService;
            this.contactService = contactService;
            this.venueService = venueService;
            this.settingService = settingService;
            this.tradingBookService = tradingBookService;
            this.logger = logger;
        }

        public async Task<TradeDataObject> ConvertTradeAsync(OwnTrade m7Trade, string apiKey)
        {
            try
            {
                var tradeDateTime = m7Trade.execTimeUTC;
                var tradeId = m7Trade.tradeId;
                var accountId = m7Trade.acctId;
                var userCode = m7Trade.usrCode;
                var quantity = m7Trade.qty;
                var price = m7Trade.px;
                var side = m7Trade.side;
                var deliveryAreaId = m7Trade.dlvryAreaId;
                var deliveryStart = m7Trade.Contract.dlvryStartUTC;
                var deliveryEnd = m7Trade.Contract.dlvryEndUTC;
                var exchangeId = m7Trade.Product.exchangeId;
                var aggressorIndicator = m7Trade.aggressorIndicator?.ToUpper();

                var allMappings = (await mappingService.GetMappingsViaApiKeyAsync(apiKey)).Data.ToDictionary(k => k.MappingKey, v => v);
                var allSettings = (await settingService.GetSettingsViaApiKeyAsync(apiKey)).Data.ToDictionary(k => k.SettingName, v => v);

                var tradeStatus = "Live";
                var quantityType = "Fixed";
                var priceType = "Fixed";
                var buySell = MapSideToBuySell(side);

                var commodityMapping = MapDeliveryAreaToCommodityAsync(deliveryAreaId, allMappings);
                var fingerprintTask = MapCommodityToProductAsync(commodityMapping?.MappingTo, deliveryStart, deliveryEnd, apiKey);
                var internalPartyTask = im7PartyService.MapInternalPartyAsync(accountId, allMappings, allSettings, apiKey);
                var counterpartyTask = im7PartyService.MapCounterpartyAsync(exchangeId, allMappings, allSettings, apiKey);

                var fingerprint = await fingerprintTask;
                var internalParty = await internalPartyTask;
                var counterparty = await counterpartyTask;

                var tradingBook = await tradingBookService.MapTradingBookAsync(deliveryAreaId, allMappings, allSettings, apiKey);
                var internalTrader = await contactService.MapInternalTraderAsync(userCode, allMappings, allSettings, apiKey);
                var venue = await venueService.MapVenueAsync(exchangeId, allMappings, allSettings, apiKey);

                if (internalParty is null)
                {
                    throw new DataException("Could not map InternalParty");
                }

                if (counterparty is null)
                {
                    throw new DataException("Could not map Counterparty");
                }

                if (tradingBook is null)
                {
                    throw new DataException("Could not map TradingBook");
                }

                if (internalTrader is null)
                {
                    throw new DataException("Could not map InternalTrader");
                }

                if (venue is null)
                {
                    throw new DataException("Could not map Venue");
                }

                var trade = new TradeDataObject
                {
                    TradeDateTime = tradeDateTime,
                    TradeStatus = tradeStatus,
                    BuySell = buySell,
                    Product = fingerprint?.ProductDataObject,
                    TradingBook = tradingBook,
                    InternalParty = internalParty,
                    InternalTrader = internalTrader,
                    Counterparty = counterparty,
                    CounterpartyReference = tradeId.ToString(),
                    Buyer = buySell == "Buy"
                        ? internalParty
                        : counterparty,
                    Seller = buySell == "Sell"
                        ? internalParty
                        : counterparty,
                    Exchange = counterparty,
                    ExchangeReference = tradeId.ToString(),
                    Initiator = aggressorIndicator == "Y"
                        ? counterparty
                        : internalParty,
                    Aggressor = aggressorIndicator == "Y"
                        ? internalParty
                        : counterparty,
                    Venue = venue,
                    Quantity = new TradeQuantity
                    {
                        Quantity = quantity,
                        QuantityType = quantityType,
                        QuantityUnit = fingerprint?.ProductDataObject?.QuantityUnit
                    },
                    Price = new TradePrice
                    {
                        Price = price,
                        PriceType = priceType,
                        PriceUnit = fingerprint?.ProductDataObject?.PriceUnit
                    }
                };

                return trade;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating trade from M7 ({Message})", ex.Message);
                throw;
            }
        }

        private async Task<FingerprintResponse> MapCommodityToProductAsync(string commodity, DateTime start, DateTime end, string apiKey)
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

        private static MappingDataObject MapDeliveryAreaToCommodityAsync(string deliveryAreaId, IReadOnlyDictionary<string, MappingDataObject> allMappings)
        {
            return allMappings.ContainsKey("M7_Commodity")
                ? allMappings["M7_Commodity"]
                : throw new DataException($"Could not map Delivery Area '{deliveryAreaId}' to M7_Commodity");
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
