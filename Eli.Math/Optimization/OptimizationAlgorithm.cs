namespace Eli.Math.Optimization;

public interface OptimizationAlgorithm
{
}

public abstract class OptimizationAlgorithm<TParameterConfig> : OptimizationAlgorithm
{
    public abstract double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] functionParameters, TParameterConfig optimizationConfig);

    public double Optimize(IEnumerable<(double, double)> points, Func<double, double, double> f, double parameter, TParameterConfig optimizationConfig) => Optimize(points, (x, p) => f(x, p[0]), [parameter], optimizationConfig)[0];

    public (double, double) Optimize(IEnumerable<(double, double)> points, Func<double, double, double, double> f, double parameter0, double parameter1, TParameterConfig optimizationConfig)
    {
        var parameters = Optimize(points, (double x, double[] p) => f(x, p[0], p[1]), [parameter0, parameter1], optimizationConfig);
        return (parameters[0], parameters[1]);
    }

    public (double, double, double) Optimize(IEnumerable<(double, double)> points, Func<double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, TParameterConfig optimizationConfig)
    {
        var parameters = Optimize(points, (double x, double[] p) => f(x, p[0], p[1], p[2]), [parameter0, parameter1, parameter2], optimizationConfig);
        return (parameters[0], parameters[1], parameters[2]);
    }

    public (double, double, double, double) Optimize(IEnumerable<(double, double)> points, Func<double, double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double parameter3, TParameterConfig optimizationConfig)
    {
        var parameters = Optimize(points, (double x, double[] p) => f(x, p[0], p[1], p[2], p[3]), [parameter0, parameter1, parameter2, parameter3], optimizationConfig);
        return (parameters[0], parameters[1], parameters[2], parameters[3]);
    }

    public (double, double, double, double, double) Optimize(IEnumerable<(double, double)> points, Func<double, double, double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double parameter3, double parameter4, TParameterConfig optimizationConfig)
    {
        var parameters = Optimize(points, (double x, double[] p) => f(x, p[0], p[1], p[2], p[3], p[4]), [parameter0, parameter1, parameter2, parameter3], optimizationConfig);
        return (parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
    }

    public (double, double, double, double, double, double) Optimize(IEnumerable<(double, double)> points, Func<double, double, double, double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double parameter3, double parameter4, double parameter5, TParameterConfig optimizationConfig)
    {
        var parameters = Optimize(points, (double x, double[] p) => f(x, p[0], p[1], p[2], p[3], p[4], p[5]), [parameter0, parameter1, parameter2, parameter3], optimizationConfig);
        return (parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
    }
}
