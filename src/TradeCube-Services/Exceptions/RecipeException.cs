using System;

namespace TradeCube_Services.Exceptions
{
    public class RecipeException : Exception
    {
        public RecipeException(string message) : base(message)
        {
        }
    }
}