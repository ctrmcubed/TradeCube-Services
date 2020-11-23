using System;

namespace TradeCube_Services.DataObjects
{
    public class TradeDataObject
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }
        public DateTime TradeDateTime { get; set; }
        public string TradeStatus { get; set; }
        public string BuySell { get; set; }
        public TradingBookDataObject TradingBook { get; set; }
        public decimal QuantityUnit { get; set; }
        public ContractDataObject Contract { get; set; }
        public ProductDataObject Product { get; set; }
        public PriceDataObject Price { get; set; }
        public QuantityDataObject Quantity { get; set; }
        public PartyDataObject Buyer { get; set; }
        public PartyDataObject Seller { get; set; }
    }
}
