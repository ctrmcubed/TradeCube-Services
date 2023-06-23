using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

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

        [BsonIgnoreIfNull] 
        public ContractDataObject Contract { get; set; }

        public ProductDataObject Product { get; set; }

        [BsonIgnoreIfNull]
        public PartyDataObject InternalParty { get; set; }

        [BsonIgnoreIfNull]
        public ContactDataObject InternalTrader { get; set; }

        public PartyDataObject Counterparty { get; set; }

        [BsonIgnoreIfNull]
        public ContactDataObject CounterpartyTrader { get; set; }

        [BsonIgnoreIfNull]
        public ContactDataObject CounterpartyContact { get; set; }

        [BsonIgnoreIfNull]
        public string CounterpartyReference { get; set; }

        [BsonIgnoreIfNull]
        public PartyDataObject Buyer { get; set; }

        [BsonIgnoreIfNull]
        public PartyDataObject Seller { get; set; }

        [BsonIgnoreIfNull]
        public PartyDataObject Exchange { get; set; }

        [BsonIgnoreIfNull]
        public ContactDataObject ExchangeContact { get; set; }

        [BsonIgnoreIfNull]
        public string ExchangeReference { get; set; }

        [BsonIgnoreIfNull] 
        public PartyDataObject Broker { get; set; }

        [BsonIgnoreIfNull]
        public ContactDataObject BrokerContact { get; set; }

        [BsonIgnoreIfNull]
        public string BrokerReference { get; set; }

        [BsonIgnoreIfNull]
        public PartyDataObject ClearingHouse { get; set; }

        [BsonIgnoreIfNull] 
        public ContactDataObject ClearingHouseContact { get; set; }

        [BsonIgnoreIfNull]
        public string ClearingHouseReference { get; set; }

        [BsonIgnoreIfNull]
        public PartyDataObject Initiator { get; set; }

        [BsonIgnoreIfNull]
        public PartyDataObject Aggressor { get; set; }

        [BsonIgnoreIfNull]
        public PartyDataObject Beneficiary { get; set; }

        [BsonIgnoreIfNull]
        public VenueDataObject Venue { get; set; }

        public TradeQuantity Quantity { get; set; }

        public TradePrice Price { get; set; }
        
        [BsonIgnoreIfNull] 
        public OrderDataObject Order { get; set; }

        [BsonIgnoreIfNull]
        // ReSharper disable once InconsistentNaming
        public string UTI { get; set; }

        [BsonIgnoreIfNull]
        public string Notes { get; set; }

        [BsonIgnoreIfNull]
        public List<FeeData> Fees { get; set; }

        [BsonIgnoreIfNull]
        public List<CreditData> Credits { get; set; }

        [BsonIgnoreIfNull]
        public List<AttachmentUpload> Attachments { get; set; }

        [BsonIgnoreIfNull]
        public List<VolumeAllocation> Allocations { get; set; }

        [BsonIgnoreIfNull]
        public List<string> Tags { get; set; }

        [BsonIgnoreIfNull]
        public ExternalFieldsType External { get; set; }

        [BsonIgnoreIfNull]
        public TradeExtension Extension { get; set; }

        [BsonIgnoreIfNull]
        public ClonedFrom ClonedFrom { get; set; }
    }
}
        