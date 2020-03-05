using System;

namespace TradeCube_Services.Helpers
{
    public static class EnvironmentVariableHelper
    {
        public static int? GetIntEnvironmentVariable(string environmentVariable)
        {
            var variable = Environment.GetEnvironmentVariable(environmentVariable);

            if (int.TryParse(variable, out var intVariable))
            {
                return intVariable;
            }

            return null;
        }
    }
}
