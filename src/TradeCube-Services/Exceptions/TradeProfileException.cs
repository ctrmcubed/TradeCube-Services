using System;

namespace TradeCube_Services.Exceptions
{
    public class TradeProfileException : Exception
    {
        public TradeProfileException(string message) : base(message)
        {
        }
    }
}