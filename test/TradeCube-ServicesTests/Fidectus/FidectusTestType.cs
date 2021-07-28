namespace TradeCube_ServicesTests.Fidectus
{
    public class FidectusTestType
    {
        public int Test { get; set; }
        public string Description { get; set; }
        public FidectusTestRequest Inputs { get; set; }
        public TradeConfirmationResult ExpectedResults { get; set; }
    }
}