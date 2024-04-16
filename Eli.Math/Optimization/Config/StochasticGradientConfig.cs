namespace Eli.Math.Optimization.Config;

public class StochasticGradientConfig : GradientConfig
{
    public int BatchSize { get; set; } = 10;
    public double LearningRateFactor { get; set; } = 0.3;
}
