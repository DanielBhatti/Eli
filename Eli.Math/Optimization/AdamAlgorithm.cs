using Eli.Math.Differentiation;
using Eli.Math.Optimization.Config;

namespace Eli.Math.Optimization;

public class AdamAlgorithm : OptimizationAlgorithm<AdamConfig>
{
    public AdamAlgorithm(AdamConfig config) : base(config) { }

    public override double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] parameters)
    {
        if(Config.BatchSize <= 0) throw new ArgumentException("Batch size must be greater than 0.", nameof(Config.BatchSize));

        var learningRate = Config.LearningRate;
        var beta1 = Config.Beta1;
        var beta2 = Config.Beta2;
        var epsilon = Config.Epsilon;
        var batchSize = Config.BatchSize;

        var m = new double[parameters.Length];
        var v = new double[parameters.Length];
        var t = 0;

        var nextParameters = new double[parameters.Length];
        var pointsList = points.ToList();
        var totalPoints = pointsList.Count;
        var random = new Random(Config.Seed);

        while(true)
        {
            var shuffledIndices = Enumerable.Range(0, totalPoints).OrderBy(x => random.Next()).ToList();

            for(var batchStart = 0; batchStart < totalPoints; batchStart += batchSize)
            {
                t += 1;
                var batchIndices = shuffledIndices.Skip(batchStart).Take(batchSize).ToList();
                if(batchIndices.Count == 0) break;

                var subset = batchIndices.Select(index => pointsList[index]).ToList();
                var gradient = Gradient.Compute(subset, f, parameters, Config.Epsilon);

                for(var i = 0; i < gradient.Length; i++)
                {
                    m[i] = beta1 * m[i] + (1 - beta1) * gradient[i];
                    v[i] = beta2 * v[i] + (1 - beta2) * gradient[i] * gradient[i];

                    var mHat = m[i] / (1 - System.Math.Pow(beta1, t));
                    var vHat = v[i] / (1 - System.Math.Pow(beta2, t));

                    nextParameters[i] = parameters[i] - learningRate * mHat / (System.Math.Sqrt(vHat) + epsilon);
                }

                var totalError = 0.0;
                for(var i = 0; i < nextParameters.Length; i++) totalError += System.Math.Abs(nextParameters[i] - parameters[i]);

                if(totalError < Config.StoppingTolerance || t > Config.MaxIterations) return nextParameters;

                for(var i = 0; i < nextParameters.Length; i++) parameters[i] = nextParameters[i];
            }

            if(t > Config.MaxIterations) break;
        }

        return nextParameters;
    }
}
