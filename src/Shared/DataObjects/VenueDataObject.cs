using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Shared.DataObjects
{
    public class VenueDataObject
    {
        public string Venue { get; init; }
        public string VenueLongName { get; init; }
        public List<string> VenueType { get; init; }
        public List<string> Countries { get; init; }
        public string PrimaryCountry { get; init; }
        public string PrimaryCurrency { get; init; }
        public CommodityDataObject PrimaryCommodity { get; set; }
        public string Image { get; set; }
        public List<string> Commodities { get; init; }
        
        // ReSharper disable once InconsistentNaming
        public string MIC { get; init; }

        public VisibilityType Visibility { get; init; }
    
        public string CollectionName() => "venue";
        public string GetKeyName() => nameof(Venue);
        public string GetKeyValue() => Venue;

        public Expression<Func<VenueDataObject, bool>> KeyExpression() =>
            x => x.Venue == Venue;
    }
}