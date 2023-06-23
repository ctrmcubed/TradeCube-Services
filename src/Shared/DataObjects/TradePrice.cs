using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects;

[BsonIgnoreExtraElements]
public class TradePrice
{
    public string PriceType { get; set; }
    public string PriceIndex { get; set; }
    public decimal? Price { get; set; }
    public decimal? Premium { get; set; }
    public bool PremiumPercent { get; set; }
    public PriceUnitDataObject PriceUnit { get; set; }
    public PriceUnitDataObject PremiumPriceUnit { get; set; }
    public IEnumerable<ProfileType> PriceProfile { get; set; }
}