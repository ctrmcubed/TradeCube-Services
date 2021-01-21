namespace Shared.Messages
{
    public class TradeSummaryResponse
    {
        public decimal? TotalVolume { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? WeightedAveragePrice { get; set; }
        public string TotalVolumeUnit { get; set; }
        public string TotalValueCurrency { get; set; }
        public string WeightedAveragePriceUnit { get; set; }
    }
}
