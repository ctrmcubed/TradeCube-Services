using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects;

[BsonIgnoreExtraElements]
public class TradeQuantity
{
    public string QuantityType { get; set; }
    public decimal? Quantity { get; init; }
    public QuantityUnitDataObject QuantityUnit { get; set; }
}