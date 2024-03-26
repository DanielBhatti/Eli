using Eli.Math.Optimization;

namespace Eli.Math.Differentiation;

public static class Gradient
{
    public static double[] Compute(Func<double[], double> f, double[] inputs, double epsilon = 1e-6)
    {
        var gradient = new double[inputs.Length];
        for(var i = 0; i < inputs.Length; i++)
        {
            var inputsPlusEpsilon = (double[])inputs.Clone();
            var inputsMinusEpsilon = (double[])inputs.Clone();

            inputsPlusEpsilon[i] += epsilon;
            inputsMinusEpsilon[i] -= epsilon;

            gradient[i] = (f(inputsPlusEpsilon) - f(inputsMinusEpsilon)) / (2 * epsilon);
        }

        return gradient;
    }

    public static double[] Compute(IEnumerable<(double x, double y)> points, Func<double, double[], double> f, double[] parameters, double epsilon)
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
}
