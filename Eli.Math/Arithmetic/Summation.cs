using System.Numerics;

namespace Eli.Math.Arithmetic;

public static class Summation
{
    public static T KahanSum<T>(this IEnumerable<T> values) where T : INumber<T>
    {
        var sum = T.Zero;
        var c = T.Zero;
        foreach(var value in values)
        {
            var y = value - c;
            var t = sum + y;
            c = t - sum - y;
            sum = t;
        }
        return sum;
    }
}
