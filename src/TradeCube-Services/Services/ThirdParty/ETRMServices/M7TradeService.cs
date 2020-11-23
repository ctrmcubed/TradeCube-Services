using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Xml.Linq;
using TradeCube_Services.DataObjects;
using TradeCube_Services.Extensions;

namespace TradeCube_Services.Services.ThirdParty.ETRMServices
{
    public class M7TradeService : IM7TradeService
    {
        private readonly IMappingService mappingService;
        private readonly ILogger<M7TradeService> logger;

        public M7TradeService(IMappingService mappingService, ILogger<M7TradeService> logger)
        {
            this.mappingService = mappingService;
            this.logger = logger;
        }

        public async Task<TradeDataObject> ConvertTrade(XElement m7Trade, string apiKey)
        {
            try
            {
                var tradeId = m7Trade?.Attribute("tradeId")?.Value;
                var contractId = m7Trade?.Attribute("contractId")?.Value;
                var execTimeUtc = m7Trade?.Attribute("execTimeUTC")?.Value;
                var side = m7Trade?.Attribute("side")?.Value;
                var deliveryAreaId = m7Trade?.Attribute("dlvryAreaId")?.Value;

                // Mappings
                var tradeDateTime = execTimeUtc.FromIso8601DateTime();
                var tradeStatus = "Live";
                var buySell = MapSideToBuySell(side);
                var tradingBook = "";

                var commodityMappingKey = "M7_Commodity";
                var commodity = await mappingService.Mapping(commodityMappingKey, deliveryAreaId, apiKey);

                return new TradeDataObject
                {
                    TradeReference = tradeId,
                    TradeLeg = 1,
                    TradeDateTime = tradeDateTime,
                    TradeStatus = tradeStatus,
                    BuySell = buySell,
                    TradingBook = new TradingBookDataObject
                    {
                        TradingBook = tradingBook
                    },
                    Product = new ProductDataObject
                    {

                    },
                    Contract = new ContractDataObject
                    {
                        ContractReference = contractId
                    }
                };
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
