using System;

public static class MathfExtension
{
    public static bool IsBetween(this float testValue, double bound1, double bound2)
    {
        return (testValue >= Math.Min(bound1, bound2) && testValue <= Math.Max(bound1, bound2));
    }
}