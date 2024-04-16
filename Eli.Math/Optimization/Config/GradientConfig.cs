namespace Eli.Math.Optimization.Config;

public class GradientConfig
{
    public int Seed { get; set; } = 573845789;
    public double StoppingTolerance { get; set; } = 1e-6;
    public double LearningRate { get; set; } = 1e-5;
    public double Epsilon { get; set; } = 1e-6;
    public long MaxIterations { get; set; } = 10_000;
}
