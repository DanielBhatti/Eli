using Eli.Math.Optimization;
using Eli.Math.Optimization.Config;

namespace Eli.Math;

public static class CurveFit
{
    public static (double, double, double) FitLaplaceParameters<TOptimizationAlgorithm, TConfig>(IEnumerable<(double, double)> points, StochasticGradientConfig? optimizationConfig = null, double? maxHeight = null, double? centerLocation = null, double? diversity = null, AlgorithmName algorithmName = 0) => Fit(points, SpecialFunction.Laplace, maxHeight ?? points.Max(p => p.Item2), centerLocation ?? points.Sum(p => p.Item1 * p.Item2) / points.Sum(p => p.Item2), diversity ?? points.StandardDeviation(p => p.Item1) / 10, algorithmName, optimizationConfig ?? new StochasticGradientConfig());

    public static (double, double, double) FitGuassianParameters(IEnumerable<(double, double)> points, StochasticGradientConfig? optimizationConfig = null, double? amplitude = null, double? mean = null, double? standardDeviation = null, AlgorithmName algorithm = 0) => Fit(points, SpecialFunction.Gaussian, amplitude ?? points.Max(p => p.Item2) - points.Min(p => p.Item2), mean ?? points.Sum(p => p.Item1 * p.Item2) / points.Sum(p => p.Item2), standardDeviation ?? points.StandardDeviation(p => p.Item1) / 10, algorithm, optimizationConfig ?? new StochasticGradientConfig());

    public static double[] Fit(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] parameters, AlgorithmName algorithm, StochasticGradientConfig optimizationConfig) => algorithm switch
    {
        AlgorithmName.GradientDescent => new GradientDescent(optimizationConfig).Optimize(points, f, parameters),
        AlgorithmName.StochasticGradientDescent => new StochasticGradientDescent(optimizationConfig).Optimize(points, f, parameters),
        //AlgorithmName.GeneticAlgorithm => new GeneticAlgorithm(optimizationConfig).Optimize(points, f, parameters),
        _ => throw new ArgumentException("Unsupported optimization algorithm", nameof(algorithm)),
    };

    public static double Fit(IEnumerable<(double, double)> points, Func<double, double, double> f, double parameter, AlgorithmName algorithm, StochasticGradientConfig optimizationConfig) => Fit(points, (x, p) => f(x, p[0]), [parameter], algorithm, optimizationConfig)[0];

    public static (double, double) Fit(IEnumerable<(double, double)> points, Func<double, double, double, double> f, double parameter0, double parameter1, AlgorithmName algorithm, StochasticGradientConfig optimizationConfig)
    {
        var parameters = Fit(points, (x, p) => f(x, p[0], p[1]), [parameter0, parameter1], algorithm, optimizationConfig);
        return (parameters[0], parameters[1]);
    }

    public static (double, double, double) Fit(IEnumerable<(double, double)> points, Func<double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, AlgorithmName algorithm, StochasticGradientConfig optimizationConfig)
    {
        var parameters = Fit(points, (x, p) => f(x, p[0], p[1], p[2]), [parameter0, parameter1, parameter2], algorithm, optimizationConfig);
        return (parameters[0], parameters[1], parameters[2]);
    }

    public static (double, double, double, double) Fit(IEnumerable<(double, double)> points, Func<double, double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double parameter3, AlgorithmName algorithm, StochasticGradientConfig optimizationConfig)
    {
        var parameters = Fit(points, (x, p) => f(x, p[0], p[1], p[2], p[3]), [parameter0, parameter1, parameter2, parameter3], algorithm, optimizationConfig);
        return (parameters[0], parameters[1], parameters[2], parameters[3]);
    }
}
