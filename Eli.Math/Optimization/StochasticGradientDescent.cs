namespace Eli.Math.Optimization;

public class StochasticGradientDescent : GradientDescent
{
    private static Random Random { get; } = new();

    public static new double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] parameters, double epsilon, double learningRate, double tolerance, long maxIterations) => Optimize(points, f, parameters, epsilon, learningRate, tolerance, maxIterations, points.Count() / 10, 0.3);

    public static double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] parameters, double epsilon, double initialLearningRate, double tolerance, long maxIterations, int batchSize, double learningRateFactor)
    {
        if(batchSize <= 0) throw new ArgumentException("Batch size must be greater than 0.", nameof(batchSize));

        var learningRate = initialLearningRate;
        var nextParameters = new double[parameters.Length];
        var n = 0;
        var pointsList = points.ToList();
        var totalPoints = pointsList.Count;
        var random = new Random();
        var previousTotalError = double.MaxValue;
        var learningRateAdjustmentFactor = learningRateFactor;

        while(true)
        {
            var shuffledIndices = Enumerable.Range(0, totalPoints).OrderBy(x => random.Next()).ToList();

            for(var batchStart = 0; batchStart < totalPoints; batchStart += batchSize)
            {
                n += 1;
                var batchIndices = shuffledIndices.Skip(batchStart).Take(batchSize).ToList();
                if(batchIndices.Count == 0) break;

                var subset = batchIndices.Select(index => pointsList[index]).ToList();
                var gradient = ComputeGradient(subset, f, parameters, epsilon);

                for(var i = 0; i < nextParameters.Length; i++) nextParameters[i] = parameters[i] - learningRate * gradient[i];

                var totalError = 0.0;
                for(var i = 0; i < nextParameters.Length; i++) totalError += System.Math.Abs(nextParameters[i] - parameters[i]);

                if(totalError < tolerance || n > maxIterations) return nextParameters;
                if(totalError >= previousTotalError) learningRate *= learningRateAdjustmentFactor;
                previousTotalError = totalError;

                for(var i = 0; i < nextParameters.Length; i++) parameters[i] = nextParameters[i];
            }
            if(n > maxIterations) break;
        }
        return nextParameters;
    }
}
