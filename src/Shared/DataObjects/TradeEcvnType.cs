using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;

public class TradeEcvnType
{
    public string Platform { get; init; }
    public string PlatformStatus { get; init; }
    public string Status { get; init; }
    public string SubmissionReference { get; init; }
    public DateTime? Initiated { get; init; }
    public DateTime? StatusLastChecked { get; init; }
        
    [JsonPropertyName("InternalPartyBSCPartyId")]
    [BsonElement("InternalPartyBSCPartyId")]
    public string InternalPartyBscPartyId { get; init; }
        
    public string InternalPartyProdConFlag { get; init; }
        
    [JsonPropertyName("CounterpartyBSCPartyId")]
    [BsonElement("CounterpartyBSCPartyId")]
    public string CounterpartyBscPartyId { get; init; }
        
    public string CounterpartyProdConFlag { get; init; }
        
    public DateTime? EffectiveFromDate { get; init; }
    public DateTime? EffectiveToDate { get; init; }
        
    [JsonPropertyName("ECVNProfile")]
    [BsonElement("ECVNProfile")]
    public IEnumerable<TradeEcvnProfile> EcvnProfile { get; init; }
}