namespace Eli.Math.Optimization.Config;

public class SimulatedAnnealingConfig
{
    public double InitialTemperature { get; }
    public double CoolingRate { get; }
    public int MaxIterations { get; }

    public SimulatedAnnealingConfig(double initialTemperature, double coolingRate, int maxIterations)
    {
        InitialTemperature = initialTemperature;
        CoolingRate = coolingRate;
        MaxIterations = maxIterations;
    }
}