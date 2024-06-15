namespace Eli.Math.Optimization.Config;

public class SimulatedAnnealingConfig
{
    public double InitialTemperature { get; set; } = 100.0;
    public double CoolingRate { get; set; } = 1.0;
    public int MaxIterations { get; set; } = 100;
}