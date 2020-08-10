﻿using System.Collections.Generic;

namespace TradeCube_Services.Messages
{
    public class TradeProfileResponse
    {
        public string TradeReference { get; set; }
        public int TradeLeg { get; set; }
        public string ProfileFormat { get; set; }
        public string Currency { get; set; }
        public string EnergyUnit { get; set; }
        public IEnumerable<VolumeProfileResponse> VolumeProfile { get; set; }
        public IEnumerable<PriceProfileResponse> PriceProfile { get; set; }
    }
}