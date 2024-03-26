using Eli.Math.Differentiation;
using Eli.Math.Optimization.Config;

namespace Eli.Math.Optimization;

public class GradientDescent : OptimizationAlgorithm<OptimizationConfig>
{
    public override double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] functionParameters, OptimizationConfig optimizationConfig)
    {
        var nextParameters = new double[functionParameters.Length];
        var n = 0;
        while(true)
        {
            n += 1;
            var gradient = Gradient.Compute(points, f, functionParameters, optimizationConfig.Epsilon);
            for(var i = 0; i < nextParameters.Length; i++) nextParameters[i] = functionParameters[i] - optimizationConfig.LearningRate * gradient[i];
            var totalError = 0.0;
            for(var i = 0; i < nextParameters.Length; i++) totalError += System.Math.Abs(nextParameters[i] - functionParameters[i]);
            if(totalError < optimizationConfig.StoppingTolerance || n > optimizationConfig.MaxIterations) break;
            for(var i = 0; i < nextParameters.Length; i++) functionParameters[i] = nextParameters[i];
        }
        return nextParameters;
    }
}
