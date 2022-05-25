using System.Collections.Generic;

namespace Shared.DataObjects
{
    public class SettlementRuleDataObject
    {
        public string SettlementRule { get; set; }
        public string SettlementRuleLongName { get; set; }
        public List<string> Commodities { get; set; }
        public string AnchorDateType { get; set; }
        public int? SkipDays { get; set; }
        public string SkipNonWorkingDayCalendar { get; set; }
        public bool SkipSaturdaySunday { get; set; }
        public List<int> SkipWeekdays { get; set; }
    }
}