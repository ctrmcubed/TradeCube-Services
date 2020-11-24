using System;
using System.Data;

namespace TradeCube_Services.Helpers
{
    public static class StringHelper
    {
        public static string StringToBase64(string text)
        {
            var encodedBytes = System.Text.Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(encodedBytes);
        }

        public static string Base64ToString(string text)
        {
            var decodedBytes = Convert.FromBase64String(text);
            return System.Text.Encoding.UTF8.GetString(decodedBytes);
        }

        public static decimal SafeParse(string value)
        {
            if (decimal.TryParse(value, out var parsedValue))
            {
                return parsedValue;
            }

            throw new DataException($"Could not parse decimal '{value}'");
        }
    }
}
