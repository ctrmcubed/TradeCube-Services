﻿using System;

namespace Shared.DataObjects
{
    public class ProductSettlement
    {
        public string SettlementType { get; set; }
        public string SettlementPeriod { get; set; }
        public DateTime? SettlementDate { get; set; }
    }
}