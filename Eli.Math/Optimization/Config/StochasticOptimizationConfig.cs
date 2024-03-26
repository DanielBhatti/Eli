namespace Eli.Math.Optimization.Config;

public class StochasticOptimizationConfig : OptimizationConfig
{
    public int BatchSize { get; set; }
    public double LearningRateFactor { get; set; }
}
