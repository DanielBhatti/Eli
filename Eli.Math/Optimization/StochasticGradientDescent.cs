using Eli.Math.Differentiation;
using Eli.Math.Optimization.Config;

namespace Eli.Math.Optimization;

public class StochasticGradientDescent : OptimizationAlgorithm<StochasticOptimizationConfig>
{
    public override double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] parameters, StochasticOptimizationConfig optimizationParameters) 
    {
        if(optimizationParameters.BatchSize <= 0) throw new ArgumentException("Batch size must be greater than 0.", nameof(optimizationParameters.BatchSize));

        var learningRate = optimizationParameters.LearningRate;
        var nextParameters = new double[parameters.Length];
        var n = 0;
        var pointsList = points.ToList();
        var totalPoints = pointsList.Count;
        var random = new Random(optimizationParameters.Seed);
        var previousTotalError = double.MaxValue;
        var learningRateAdjustmentFactor = optimizationParameters.LearningRateFactor;

        while(true)
        {
            var shuffledIndices = Enumerable.Range(0, totalPoints).OrderBy(x => random.Next()).ToList();

            for(var batchStart = 0; batchStart < totalPoints; batchStart += optimizationParameters.BatchSize)
            {
                n += 1;
                var batchIndices = shuffledIndices.Skip(batchStart).Take(optimizationParameters.BatchSize).ToList();
                if(batchIndices.Count == 0) break;

                var subset = batchIndices.Select(index => pointsList[index]).ToList();
                var gradient = Gradient.Compute(subset, f, parameters, optimizationParameters.Epsilon);

                for(var i = 0; i < nextParameters.Length; i++) nextParameters[i] = parameters[i] - learningRate * gradient[i];

                var totalError = 0.0;
                for(var i = 0; i < nextParameters.Length; i++) totalError += System.Math.Abs(nextParameters[i] - parameters[i]);

                if(totalError < optimizationParameters.StoppingTolerance || n > optimizationParameters.MaxIterations) return nextParameters;
                if(totalError >= previousTotalError) learningRate *= learningRateAdjustmentFactor;
                previousTotalError = totalError;

                for(var i = 0; i < nextParameters.Length; i++) parameters[i] = nextParameters[i];
            }
            if(n > optimizationParameters.MaxIterations) break;
        }
        return nextParameters;
    }
}
