namespace Eli.Math.Optimization.Config;

public class AdamConfig
{
    public int Seed { get; set; } = 573845789;
    public double StoppingTolerance { get; set; } = 1e-6;
    public long MaxIterations { get; set; } = 10_000;
    public double LearningRate { get; set; } = 0.001;
    public double Beta1 { get; set; } = 0.9;
    public double Beta2 { get; set; } = 0.999;
    public double Epsilon { get; set; } = 1e-8;
    public int BatchSize { get; set; } = 32;
}