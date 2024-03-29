﻿using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects
{
    public class TradeExtension
    {
        [JsonPropertyName("ECVNAgentType")]
        [BsonElement("ECVNAgentType")]
        public string EcvnAgentType { get; set; }

        [JsonPropertyName("ECVNAgentParty")]
        [BsonElement("ECVNAgentParty")]
        public PartyDataObject EcvnAgentParty { get; set; }
        
        public string InternalPartyEnergyAccountType { get; set; }
        public string CounterpartyEnergyAccountType { get; set; }
        public string ReplacementRuleType { get; set; }
        public string Schedule5Type { get; set; }
        public string BuyerEnergyAccount { get; set; }
        public string SellerEnergyAccount { get; set; }
        public string ReplacementRule { get; set; }
        public string Schedule5 { get; set; }
    }
}