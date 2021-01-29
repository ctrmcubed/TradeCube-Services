using System;

namespace Shared.DataObjects
{
    public class OrderDataObject
    {
        public string OrderReference { get; set; }
        public string OrderDescription { get; set; }
        public DateTime OrderDateTime { get; set; }
        public DateTime? OrderExpiresDateTime { get; set; }
        public string TimeInForce { get; set; }
        public string FillType { get; set; }
        public string OrderType { get; set; }
        public string OrderStatus { get; set; }
        public string BuySell { get; set; }
        public CommodityDataObject Commodity { get; set; }
        public ContractDataObject Contract { get; set; }
        public ProductDataObject Product { get; set; }
        public PartyDataObject InternalParty { get; set; }
        public ContactDataObject InternalTrader { get; set; }
        public PartyDataObject Exchange { get; set; }
        public string ExchangeReference { get; set; }
        public PartyDataObject Broker { get; set; }
        public string BrokerReference { get; set; }
        public VenueDataObject Venue { get; set; }
        public decimal? MinimumQuantity { get; set; }
        public decimal? MaximumQuantity { get; set; }
        public decimal? MinimumStepQuantity { get; set; }
        public QuantityUnitDataObject QuantityUnit { get; set; }
        public PriceUnitDataObject PriceUnit { get; set; }
        public decimal? MinimumPrice { get; set; }
        public decimal? MaximumPrice { get; set; }
    }
}