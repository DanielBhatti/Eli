namespace Eli.Math.Probability;

public static class ProbabilitySeries
{
    public static double ExpectedValue(IEnumerable<double> values) => values.Sum() / values.Count();
    public static double ExpectedValue(IEnumerable<(double Probability, double Value)> data) => data.Sum(d => d.Probability * d.Value) / data.Sum(d => d.Probability);

    public static double Variance(IEnumerable<double> values)
    {
        var expectedValue = ExpectedValue(values);
        return values.Sum(v => System.Math.Pow(v - expectedValue, 2));
    }
    public static double Variance(IEnumerable<(double Probability, double Value)> data)
    {
        var expectedValue = ExpectedValue(data);
        return data.Sum(d => d.Probability * System.Math.Pow(d.Value - expectedValue, 2)) / data.Sum(d => d.Probability);
    }

    public static double StandardDeviation(IEnumerable<double> values) => System.Math.Sqrt(Variance(values));
    public static double StandardDeviation(IEnumerable<(double Probability, double Value)> data) => System.Math.Sqrt(Variance(data));
}
