using System.Text.Json.Serialization;

namespace Fidectus.Models
{
    public class Currency
    {
        [JsonPropertyName("currencyCodeType")]
        public string CurrencyCodeType { get; set; }

        public bool UseFractionUnit { get; set; }
    }
}