using System.Collections.Generic;

namespace Eli.Extensions;

public static partial class Enumerable
{
    public static IEnumerable<double> Range(double start, double end, double step)
    {
        for(var current = start; current < end; current += step) yield return current;
    }

    public static IEnumerable<decimal> Range(decimal start, decimal end, decimal step)
    {
        for(var current = start; current < end; current += step) yield return current;
    }
}