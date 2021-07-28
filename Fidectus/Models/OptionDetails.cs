using System;

namespace Fidectus.Models
{
    public class OptionDetails
    {
        public string OptionHolder { get; set; }
        public string OptionStyle { get; set; }
        public string OptionWriter { get; set; }
        public string OptionsType { get; set; }
        public float StrikePrice { get; set; }
        public float TotalPremiumValue { get; set; }
        public string IndexStrikePriceStyle { get; set; }
        public float SecondStrikePrice { get; set; }
        public float CappedPrice { get; set; }
        public float FlooredPrice { get; set; }
        public string OptionCurrency { get; set; }
        public float PremiumRate { get; set; }
        public string PremiumCurrency { get; set; }
        public PremiumUnit PremiumUnit { get; set; }
        public string PremiumPaymentDate { get; set; }
        public DateTime ExerciseDateTime { get; set; }
        public bool AutomaticExercise { get; set; }
        public bool EarlyExercise { get; set; }
        public bool WrittenConfirmationOfExercise { get; set; }
        public bool CashSettlement { get; set; }
        public PremiumPayment[] PremiumPayments { get; set; }
        public ExerciseSchedule[] ExerciseSchedule { get; set; }
    }
}