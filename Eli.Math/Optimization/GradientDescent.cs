namespace Eli.Math.Optimization;

public class GradientDescent
{
    public static double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] parameters, double epsilon, double learningRate, double tolerance, long maxIterations)
    {
        var nextParameters = new double[parameters.Length];
        var n = 0;
        while(true)
        {
            n += 1;
            var gradient = ComputeGradient(points, f, parameters, epsilon);
            for(var i = 0; i < nextParameters.Length; i++) nextParameters[i] = parameters[i] - learningRate * gradient[i];
            var totalError = 0.0;
            for(var i = 0; i < nextParameters.Length; i++) totalError += System.Math.Abs(nextParameters[i] - parameters[i]);
            if(totalError < tolerance || n > maxIterations) break;
            for(var i = 0; i < nextParameters.Length; i++) parameters[i] = nextParameters[i];
        }
        return nextParameters;
    }

    public static double Optimize(IEnumerable<(double, double)> points, Func<double, double, double> f, double parameter, double epsilon = 1e-7, double learningRate = 1e-2, double tolerance = 1e-5, long maxIterations = 1000) => Optimize(points, (x, p) => f(x, p[0]), [parameter], epsilon, learningRate, tolerance, maxIterations)[0];

    public static (double, double) Optimize(IEnumerable<(double, double)> points, Func<double, double, double, double> f, double parameter0, double parameter1, double epsilon = 1e-7, double learningRate = 1e-2, double tolerance = 1e-5, long maxIterations = 1000)
    {
        var parameters = Optimize(points, (double x, double[] p) => f(x, p[0], p[1]), [parameter0, parameter1], epsilon, learningRate, tolerance, maxIterations);
        return (parameters[0], parameters[1]);
    }

    public static (double, double, double) Optimize(IEnumerable<(double, double)> points, Func<double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double epsilon = 1e-7, double learningRate = 1e-2, double tolerance = 1e-5, long maxIterations = 1000)
    {
        var parameters = Optimize(points, (double x, double[] p) => f(x, p[0], p[1], p[2]), [parameter0, parameter1, parameter2], epsilon, learningRate, tolerance, maxIterations);
        return (parameters[0], parameters[1], parameters[2]);
    }

    public static (double, double, double, double) Optimize(IEnumerable<(double, double)> points, Func<double, double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double parameter3, double epsilon = 1e-7, double learningRate = 1e-2, double tolerance = 1e-5, long maxIterations = 1000)
    {
        var parameters = Optimize(points, (double x, double[] p) => f(x, p[0], p[1], p[2], p[3]), [parameter0, parameter1, parameter2, parameter3], epsilon, learningRate, tolerance, maxIterations);
        return (parameters[0], parameters[1], parameters[2], parameters[3]);
    }

    public static double[] ComputeGradient(IEnumerable<(double x, double y)> points, Func<double, double[], double> f, double[] parameters, double epsilon)
    {
        var gradients = new double[parameters.Length];
        var parametersPlusEpsilon = new double[parameters.Length];
        var parametersMinusEpsilon = new double[parameters.Length];
        for(var i = 0; i < parameters.Length; i++)
        {
            for(var j = 0; j < parameters.Length; j++)
            {
                parametersPlusEpsilon[j] = parameters[j];
                parametersMinusEpsilon[j] = parameters[j];
            }

            parametersPlusEpsilon[i] *= 1 + epsilon;
            parametersMinusEpsilon[i] *= 1 - epsilon;

            var functionPlusEpsilon = ObjectiveFunction.MeanSquaredError(points.Select(p => (f(p.x, parametersPlusEpsilon), p.y)));
            var functionMinusEpsilon = ObjectiveFunction.MeanSquaredError(points.Select(p => (f(p.x, parametersMinusEpsilon), p.y)));

            gradients[i] = (functionPlusEpsilon - functionMinusEpsilon) / (2 * epsilon);
        }
        return gradients;
    }

    public static double ComputeGradient(IEnumerable<(double, double)> points, Func<double, double, double> f, double parameter, double epsilon = 1e-7) => ComputeGradient(points, (x, p) => f(x, p[0]), [parameter], epsilon)[0];

    public static (double, double) ComputeGradient(IEnumerable<(double, double)> points, Func<double, double, double, double> f, double parameter0, double parameter1, double epsilon = 1e-7)
    {
        var parameters = ComputeGradient(points, (x, p) => f(x, p[0], p[1]), [parameter0, parameter1], epsilon);
        return (parameters[0], parameters[1]);
    }

    public static (double, double, double) ComputeGradient(IEnumerable<(double, double)> points, Func<double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double epsilon = 1e-7)
    {
        var parameters = ComputeGradient(points, (x, p) => f(x, p[0], p[1], p[2]), [parameter0, parameter1, parameter2], epsilon);
        return (parameters[0], parameters[1], parameters[2]);
    }

    public static (double, double, double, double) ComputeGradient(IEnumerable<(double, double)> points, Func<double, double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double parameter3, double epsilon = 1e-7)
    {
        var parameters = ComputeGradient(points, (x, p) => f(x, p[0], p[1], p[2], p[3]), [parameter0, parameter1, parameter2, parameter3], epsilon);
        return (parameters[0], parameters[1], parameters[2], parameters[3]);
    }

    public static (double, double, double, double, double) ComputeGradient(IEnumerable<(double, double)> points, Func<double, double, double, double, double, double, double> f, double parameter0, double parameter1, double parameter2, double parameter3, double parameter4, double epsilon = 1e-7)
    {
        var parameters = ComputeGradient(points, (x, p) => f(x, p[0], p[1], p[2], p[3], p[4]), [parameter0, parameter1, parameter2, parameter3, parameter4], epsilon);
        return (parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
    }
}
