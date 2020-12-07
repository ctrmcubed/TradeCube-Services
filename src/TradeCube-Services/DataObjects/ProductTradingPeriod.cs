﻿using Newtonsoft.Json;
using System;

namespace TradeCube_Services.DataObjects
{
    public class ProductTradingPeriod
    {
        [JsonProperty("UTCStartDateTime")]
        public DateTime? UtcStartDateTime { get; set; }

        [JsonProperty("UTCEndDateTime")]
        public DateTime? UtcEndDateTime { get; set; }
    }
}