using System;
using System.Security.Cryptography;
using System.Text;

namespace TradeCube_Services.Helpers
{
    public static class HashHelper
    {
        public static int HashStringToInteger(string str)
        {
            var md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
            var toInteger = BitConverter.ToInt32(hashed, 0);

            return toInteger;
        }
    }
}
