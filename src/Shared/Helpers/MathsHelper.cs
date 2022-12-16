using System;

namespace Shared.Helpers;

public static class MathsHelper
{
    public static bool SameSign(decimal a, decimal b) =>
        Math.Sign(a) == Math.Sign(b);

    public static decimal? Round(decimal? a, int precision) =>
        a.HasValue
            ? Math.Round(a.Value, precision)
            : null;
    
    public static bool IsWithinTolerance(decimal? a, decimal? b, decimal tolerance = 0.000001m) => 
        a.HasValue && b.HasValue && Math.Abs(a.Value - b.Value) <= tolerance;
}