using Eli.Math.Optimization;
using Eli.Math.Optimization.Config;

namespace Eli.Math;

public static class CurveFit
{
    public static (double, double, double) FitLaplaceParameters(IEnumerable<(double, double)> points, double? maxHeight = null, double? centerLocation = null, double? diversity = null, AlgorithmName algorithmName = 0) => Fit(points, SpecialFunction.Laplace, maxHeight ?? points.Max(p => p.Item2), centerLocation ?? points.Sum(p => p.Item1 * p.Item2) / points.Sum(p => p.Item2), diversity ?? points.StandardDeviation(p => p.Item1) / 10, algorithmName, new StochasticOptimizationConfig());

    public static (double, double, double) FitGuassianParameters(IEnumerable<(double, double)> points, double? amplitude = null, double? mean = null, double? standardDeviation = null, AlgorithmName algorithm = 0) => Fit(points, SpecialFunction.Gaussian, amplitude ?? points.Max(p => p.Item2) - points.Min(p => p.Item2), mean ?? points.Sum(p => p.Item1 * p.Item2) / points.Sum(p => p.Item2), standardDeviation ?? points.StandardDeviation(p => p.Item1) / 10, algorithm, new StochasticOptimizationConfig());

    public static double[] Fit(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] parameters, AlgorithmName algorithm, StochasticOptimizationConfig optimizationConfig) => algorithm switch
    {
        AlgorithmName.GradientDescent => new GradientDescent().Optimize(points, f, parameters, optimizationConfig),
        AlgorithmName.StochasticGradientDescent => new StochasticGradientDescent().Optimize(points, f, parameters, optimizationConfig),
        _ => throw new ArgumentException("Unsupported optimization algorithm", nameof(algorithm)),
    };

    public static double Fit(IEnumerable<(double, double)> points, Func<double, double, double> f, double parameter, AlgorithmName algorithm, StochasticOptimizationConfig optimizationConfig) => Fit(points, (x, p) => f(x, p[0]), [parameter], algorithm, optimizationConfig)[0];

    public static (double, double) Fit(IEnumerable<(double, double)> points, Func<double, double, double, double> f, double parameter0, double parameter1, AlgorithmName algorithm, StochasticOptimizationConfig optimizationConfig)
    {
        var parameters = Fit(points, (x, p) => f(x, p[0], p[1]), [parameter0, parameter1], algorithm, optimizationConfig);
        return (parameters[0], parameters[1]);
    }

    public static (double, double, double) Fit(IEnumerable<(double, double)> points, Func<double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, AlgorithmName algorithm, StochasticOptimizationConfig optimizationConfig)
    {
        var parameters = Fit(points, (x, p) => f(x, p[0], p[1], p[2]), [parameter0, parameter1, parameter2], algorithm, optimizationConfig);
        return (parameters[0], parameters[1], parameters[2]);
    }

    public static (double, double, double, double) Fit(IEnumerable<(double, double)> points, Func<double, double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double parameter3, AlgorithmName algorithm, StochasticOptimizationConfig optimizationConfig)
    {
        var parameters = Fit(points, (x, p) => f(x, p[0], p[1], p[2], p[3]), [parameter0, parameter1, parameter2, parameter3], algorithm, optimizationConfig);
        return (parameters[0], parameters[1], parameters[2], parameters[3]);
    }
}
