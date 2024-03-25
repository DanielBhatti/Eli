using Eli.Math.Optimization;

namespace Eli.Math;

public static class CurveFit
{
    public static (double, double, double) FitLaplaceParameters(IEnumerable<(double, double)> points, double? maxHeight = null, double? centerLocation = null, double? diversity = null, double epsilon = 1e-7, double learningRate = 1e-5, double tolerance = 1e-6, long maxIterations = 10000, AlgorithmName algorithm = 0) => Fit(points, SpecialFunction.Laplace, maxHeight ?? points.Max(p => p.Item2), centerLocation ?? points.Sum(p => p.Item1 * p.Item2) / points.Sum(p => p.Item2), diversity ?? points.StandardDeviation(p => p.Item1) / 10, epsilon, learningRate, tolerance, maxIterations, algorithm);

    public static (double, double, double) FitGuassianParameters(IEnumerable<(double, double)> points, double? amplitude = null, double? mean = null, double? standardDeviation = null, double epsilon = 1e-7, double learningRate = 1e-5, double tolerance = 1e-6, long maxIterations = 10000, AlgorithmName algorithm = 0) => Fit(points, SpecialFunction.Gaussian, amplitude ?? points.Max(p => p.Item2) - points.Min(p => p.Item2), mean ?? points.Sum(p => p.Item1 * p.Item2) / points.Sum(p => p.Item2), standardDeviation ?? points.StandardDeviation(p => p.Item1) / 10, epsilon, learningRate, tolerance, maxIterations, algorithm);

    public static double[] Fit(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] parameters, double epsilon, double learningRate, double tolerance, long maxIterations, AlgorithmName algorithm = 0, int batchSize = 100) => algorithm switch
    {
        AlgorithmName.GradientDescent => GradientDescent.Optimize(points, f, parameters, epsilon, learningRate, tolerance, maxIterations),
        AlgorithmName.StochasticGradientDescent => StochasticGradientDescent.Optimize(points, f, parameters, epsilon, learningRate, tolerance, maxIterations, batchSize, 0.4),
        _ => throw new ArgumentException("Unsupported optimization algorithm", nameof(algorithm)),
    };

    public static double Fit(IEnumerable<(double, double)> points, Func<double, double, double> f, double parameter, double epsilon = 1e-7, double learningRate = 1e-4, double tolerance = 1e-5, long maxIterations = 1000, AlgorithmName algorithm = 0) => Fit(points, (x, p) => f(x, p[0]), [parameter], epsilon, learningRate, tolerance, maxIterations, algorithm)[0];

    public static (double, double) Fit(IEnumerable<(double, double)> points, Func<double, double, double, double> f, double parameter0, double parameter1, double epsilon = 1e-7, double learningRate = 1e-2, double tolerance = 1e-5, long maxIterations = 1000, AlgorithmName algorithm = 0)
    {
        var parameters = Fit(points, (x, p) => f(x, p[0], p[1]), [parameter0, parameter1], epsilon, learningRate, tolerance, maxIterations, algorithm);
        return (parameters[0], parameters[1]);
    }

    public static (double, double, double) Fit(IEnumerable<(double, double)> points, Func<double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double epsilon = 1e-7, double learningRate = 1e-2, double tolerance = 1e-5, long maxIterations = 1000, AlgorithmName algorithm = 0)
    {
        var parameters = Fit(points, (x, p) => f(x, p[0], p[1], p[2]), [parameter0, parameter1, parameter2], epsilon, learningRate, tolerance, maxIterations, algorithm);
        return (parameters[0], parameters[1], parameters[2]);
    }

    public static (double, double, double, double) Fit(IEnumerable<(double, double)> points, Func<double, double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double parameter3, double epsilon = 1e-7, double learningRate = 1e-2, double tolerance = 1e-5, long maxIterations = 1000, AlgorithmName algorithm = 0)
    {
        var parameters = Fit(points, (x, p) => f(x, p[0], p[1], p[2], p[3]), [parameter0, parameter1, parameter2, parameter3], epsilon, learningRate, tolerance, maxIterations, algorithm);
        return (parameters[0], parameters[1], parameters[2], parameters[3]);
    }
}
