using System;

namespace Shared.Helpers;

public static class EnvironmentVariableHelper
{
    public static string GetEnvironmentVariable(string environmentVariable) => 
        Environment.GetEnvironmentVariable(environmentVariable);
}