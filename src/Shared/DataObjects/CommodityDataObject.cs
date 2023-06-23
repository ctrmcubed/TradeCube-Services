using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.DataObjects
{
    public class CommodityDataObject
    {
        public string Commodity { get; init; }
        public string CommodityLongName { get; init; }
        public string Currency { get; init; }
        public string Country { get; init; }

        [BsonIgnoreIfNull] 
        public int? YearMonthOffset { get; init; }

        [BsonIgnoreIfNull] 
        public int? DayMinuteOffset { get; init; }
    
        [BsonIgnoreIfNull] 
        public string TimeNodeType { get; init; }

        [BsonIgnoreIfNull] 
        public DeliveryAreaDataObject DeliveryArea { get; set; }

        [BsonIgnoreIfNull] 
        public string Timezone { get; init; }

        [BsonIgnoreIfNull] 
        public EnergyUnitDataObject EnergyUnit { get; set; }

        [BsonIgnoreIfNull] 
        public BaseCommodityDataObject BaseCommodity { get; set; }

        [BsonIgnoreIfNull] 
        public QuantityUnitDataObject DefaultQuantityUnit { get; set; }

        [BsonIgnoreIfNull] 
        public PriceUnitDataObject DefaultPriceUnit { get; set; }

        [BsonIgnoreIfNull] 
        public List<string> RegulatoryMechanisms { get; set; }

        [BsonIgnoreIfNull] 
        public SettlementRuleDataObject DefaultBillRule { get; set; }

        [BsonIgnoreIfNull] 
        public SettlementRuleDataObject DefaultPayRule { get; set; }
    
        public string DefaultSettlementPeriod { get; init; }

        [BsonIgnoreIfNull] 
        public string Image { get; set; }

        [BsonIgnoreIfNull]
        public VisibilityType Visibility { get; init; }
    }
}