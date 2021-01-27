using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class TradeDataObject
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }
        public DateTime TradeDateTime { get; set; }
        public string TradeStatus { get; set; }
        public string BuySell { get; set; }
        public TradingBookDataObject TradingBook { get; set; }
        public ProductDataObject Product { get; set; }
        public PartyDataObject InternalParty { get; set; }
        public ContactDataObject InternalTrader { get; set; }
        public PartyDataObject Counterparty { get; set; }
        public ContractDataObject Contract { get; set; }
        public string CounterpartyReference { get; set; }
        public PriceDataObject Price { get; set; }
        public IEnumerable<CashflowType> Cashflow { get; set; }
        public QuantityDataObject Quantity { get; set; }
        public PartyDataObject Buyer { get; set; }
        public PartyDataObject Seller { get; set; }
        public PartyDataObject Exchange { get; set; }
        public string ExchangeReference { get; set; }
        public PartyDataObject Initiator { get; set; }
        public PartyDataObject Aggressor { get; set; }
        public PartyDataObject Beneficiary { get; set; }
        public VenueDataObject Venue { get; set; }

        [JsonPropertyName("UTI")]
        public string Uti { get; set; }

        public ExternalFieldsType External { get; set; }
    }
}
