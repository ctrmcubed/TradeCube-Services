using System.Collections.Generic;
using TradeCube_Services.DataObjects;

namespace TradeCube_Services.Extensions
{
    public static class AddressTypeExtensions
    {
        public static string ConcatenateAddress(this AddressType addressType, string separator = ",")
        {
            return addressType == null
                ? null
                : string.Join(separator, new List<string>
                {
                    addressType.Line1,
                    addressType.Line2,
                    addressType.Line3,
                    addressType.Line4,
                    addressType.Line5,
                    addressType.PostalCode,
                    addressType.Country
                });
        }

        public static AddressType SplitAddress(this string addressBlob)
        {
            if (addressBlob == null)
            {
                return null;
            }

            var splitAddress = StringExtensions.SplitAddress(addressBlob);

            return new AddressType
            {
                Line1 = splitAddress.line1,
                Line2 = splitAddress.line2,
                Line3 = splitAddress.line3,
                Line4 = splitAddress.line4,
                Line5 = splitAddress.line5
            };
        }
    }
}