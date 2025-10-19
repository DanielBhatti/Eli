using Eli.Math.Differentiation;
using Eli.Math.Optimization.Config;

namespace Eli.Math.Optimization;

public class StochasticGradientDescent : OptimizationAlgorithm<StochasticGradientConfig>
{
    public StochasticGradientDescent(StochasticGradientConfig config) : base(config) { }

    public override double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] parameters)
    {
        if(Config.BatchSize <= 0) throw new ArgumentException("Batch size must be greater than 0.", nameof(Config.BatchSize));

        var learningRate = Config.LearningRate;
        var nextParameters = new double[parameters.Length];
        var n = 0;
        var pointsList = points.ToList();
        var totalPoints = pointsList.Count;
        var random = new Random(Config.Seed);
        var previousTotalError = double.MaxValue;
        var learningRateAdjustmentFactor = Config.LearningRateFactor;

        while(true)
        {
            var shuffledIndices = Enumerable.Range(0, totalPoints).OrderBy(x => random.Next()).ToList();

            for(var batchStart = 0; batchStart < totalPoints; batchStart += Config.BatchSize)
            {
                n += 1;
                var batchIndices = shuffledIndices.Skip(batchStart).Take(Config.BatchSize).ToList();
                if(batchIndices.Count == 0) break;

                var subset = batchIndices.Select(index => pointsList[index]).ToList();
                var gradient = Gradient.Compute(subset, f, parameters, Config.Epsilon);

                for(var i = 0; i < nextParameters.Length; i++) nextParameters[i] = parameters[i] - learningRate * gradient[i];

                var totalError = 0.0;
                for(var i = 0; i < nextParameters.Length; i++) totalError += System.Math.Abs(nextParameters[i] - parameters[i]);

                if(totalError < Config.StoppingTolerance || n > Config.MaxIterations) return nextParameters;
                if(totalError >= previousTotalError) learningRate *= learningRateAdjustmentFactor;
                previousTotalError = totalError;

                for(var i = 0; i < nextParameters.Length; i++) parameters[i] = nextParameters[i];
            }
            if(n > Config.MaxIterations) break;
        }
        return nextParameters;
    }
}
