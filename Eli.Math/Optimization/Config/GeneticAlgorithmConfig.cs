namespace Eli.Math.Optimization.Config;

public class GeneticAlgorithmConfig
{
    public int PopulationSize { get; set; } = 50;
    public double MutationRate { get; set; } = 0.01;
    public double CrossoverRate { get; set; } = 0.7;
    public int Generations { get; set; } = 100;
}