using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.DataObjects
{
    public class TradeDataObject : DataObject
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }
        public DateTime TradeDateTime { get; set; }
        public string TradeStatus { get; set; }
        public string BuySell { get; set; }
        public bool ProfileTrade { get; set; }
        public TradingBookDataObject TradingBook { get; set; }
        public ContractDataObject Contract { get; set; }
        public ProductDataObject Product { get; set; }
        public PartyDataObject InternalParty { get; set; }
        public ContactDataObject InternalTrader { get; set; }
        public PartyDataObject Counterparty { get; set; }
        public ContactDataObject CounterpartyTrader { get; set; }
        public ContactDataObject CounterpartyContact { get; set; }
        public string CounterpartyReference { get; set; }
        public PartyDataObject Buyer { get; set; }
        public PartyDataObject Seller { get; set; }
        public PartyDataObject Exchange { get; set; }
        public ContactDataObject ExchangeContact { get; set; }
        public string ExchangeReference { get; set; }
        public PartyDataObject Broker { get; set; }
        public ContactDataObject BrokerContact { get; set; }
        public string BrokerReference { get; set; }
        public PartyDataObject ClearingHouse { get; set; }
        public ContactDataObject ClearingHouseContact { get; set; }
        public string ClearingHouseReference { get; set; }
        public PartyDataObject Initiator { get; set; }
        public PartyDataObject Aggressor { get; set; }
        public PartyDataObject Beneficiary { get; set; }
        public VenueDataObject Venue { get; set; }
        public QuantityDataObject Quantity { get; set; }
        public PriceDataObject Price { get; set; }
        public IEnumerable<CashflowType> Cashflow { get; set; }
        public OrderDataObject Order { get; set; }

        [JsonPropertyName("UTI")]
        public string Uti { get; set; }
        public string Notes { get; set; }

        public IEnumerable<FeeData> Fees { get; set; }

        public IEnumerable<CreditData> Credits { get; set; }

        public IEnumerable<FileUpload> Attachments { get; set; }

        public IEnumerable<VolumeAllocation> Allocations { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public ExternalFieldsType External { get; set; }
    }
}
