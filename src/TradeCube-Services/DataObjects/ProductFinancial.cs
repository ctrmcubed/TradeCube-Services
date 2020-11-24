using System;

namespace TradeCube_Services.DataObjects
{
    public class ProductFinancial
    {
        public string SwapType { get; set; }
        public DateTime? MaturityDate { get; set; }
        public SimpleCurveType FloatingPriceCurve { get; set; }
        public SimpleCurveType FloatingPriceCurve2 { get; set; }
    }
}