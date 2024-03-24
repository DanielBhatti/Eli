namespace Eli.Math;

public static class LinqExtensions
{
    public static double Median<T>(this IEnumerable<T> source, Func<T, double> selector, bool sample = true)
    {
        var values = source.Select(selector).ToList();
        var count = values.Count();
        if(count < 2) return 0;
        var mean = values.Average();
        var sumOfSquaresOfDifferences = values.Sum(val => (val - mean) * (val - mean));
        return sumOfSquaresOfDifferences / (sample ? count - 1 : count);
    }

    public static double Variance<T>(this IEnumerable<T> source, Func<T, double> selector, bool sample = true)
    {
        var values = source.Select(selector).ToList();
        var count = values.Count();
        if(count < 2) return 0;
        var mean = values.Average();
        var sumOfSquaresOfDifferences = values.Sum(val => (val - mean) * (val - mean));
        return sumOfSquaresOfDifferences / (sample ? count - 1 : count);
    }

    public static double StandardDeviation<T>(this IEnumerable<T> source, Func<T, double> selector, bool sample = true) => System.Math.Sqrt(Variance(source, selector, sample));
}
