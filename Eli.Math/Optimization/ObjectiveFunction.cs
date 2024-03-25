namespace Eli.Math.Optimization;

public static class ObjectiveFunction
{
    public static double MeanSquaredError(IEnumerable<(double Expected, double Actual)> values) => values.Sum(p => System.Math.Pow(p.Expected - p.Actual, 2)) / values.Count();
}
