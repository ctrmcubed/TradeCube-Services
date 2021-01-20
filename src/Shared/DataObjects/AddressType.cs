using Shared.Extensions;

namespace Shared.DataObjects
{
    public class AddressType
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }

        public string Country { get; set; }

        public string PostalCode { get; set; }


        public string ConcatenatedAddress
        {
            get => this.ConcatenateAddress("\r\n");
            set
            {
                var splitAddress = value?.SplitAddress();

                Line1 = splitAddress?.Line1;
                Line2 = splitAddress?.Line2;
                Line3 = splitAddress?.Line3;
                Line4 = splitAddress?.Line4;
                Line5 = splitAddress?.Line5;
            }
        }
    }
}