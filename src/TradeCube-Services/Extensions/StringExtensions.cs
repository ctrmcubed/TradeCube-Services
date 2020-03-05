using System;
using System.Collections.Generic;

namespace TradeCube_Services.Extensions
{
    public static class StringExtensions
    {
        public static (string line1, string line2, string line3, string line4, string line5) SplitAddress(this string addressBlob, string[] separators = null)
        {
            string GetString(IReadOnlyList<string> strings, int index)
            {
                return strings.Count > index ? strings[index] : string.Empty;
            }

            if (string.IsNullOrEmpty(addressBlob))
            {
                return (string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            }


            var result = addressBlob.Split(separators ?? new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            return (GetString(result, 0),
                GetString(result, 1),
                GetString(result, 2),
                GetString(result, 3),
                GetString(result, 4));
        }
    }
}
