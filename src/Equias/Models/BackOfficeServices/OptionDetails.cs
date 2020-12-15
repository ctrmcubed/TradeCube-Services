using System;
using System.Collections.Generic;

namespace Equias.Models.BackOfficeServices
{
    public class OptionDetails
    {
        public string OptionsType { get; set; }
        public string OptionWriter { get; set; }
        public string OptionHolder { get; set; }
        public string OptionStyle { get; set; }
        public decimal StrikePrice { get; set; }
        public string IndexStrikePriceStyle { get; set; }
        public decimal SecondStrikePrice { get; set; }
        public decimal CappedPrice { get; set; }
        public decimal FlooredPrice { get; set; }
        public string OptionCurrency { get; set; }
        public decimal PremiumRate { get; set; }
        public string PremiumCurrency { get; set; }
        public PremiumUnit PremiumUnit { get; set; }
        public decimal TotalPremiumValue { get; set; }
        public DateTime EmissionsExerciseDateTime { get; set; }
        public DateTime PremiumPaymentDate { get; set; }
        public IEnumerable<PremiumPayment> PremiumPayments { get; set; }
        public IEnumerable<ExerciseSchedule> ExerciseSchedule { get; set; }
        public bool AutomaticExercise { get; set; }
        public bool EarlyExercise { get; set; }
        public bool WrittenConfirmationOfExercise { get; set; }
        public bool CashSettlement { get; set; }
    }
}