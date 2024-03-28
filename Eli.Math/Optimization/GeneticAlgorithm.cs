using Eli.Math.Optimization.Config;

namespace Eli.Math.Optimization;

public class GeneticAlgorithm : OptimizationAlgorithm<GeneticAlgorithmConfig>
{
    private Random Random { get; } = new();

    public override double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] functionParameters, GeneticAlgorithmConfig config)
    {
        var population = new List<double[]>();
        for(var i = 0; i < config.PopulationSize; i++)
        {
            var individual = functionParameters.Select(p => p + (Random.NextDouble() - 0.5) * 2 * p).ToArray();
            population.Add(individual);
        }

        for(var generation = 0; generation < config.Generations; generation++)
        {
            var fitnessScores = population.Select(individual => points.Sum(point => System.Math.Abs(f(point.Item1, individual) - point.Item2))).ToList();

            var newPopulation = new List<double[]>();
            while(newPopulation.Count < config.PopulationSize)
            {
                var index1 = TournamentSelection(fitnessScores);
                var index2 = TournamentSelection(fitnessScores);
                if(Random.NextDouble() < config.CrossoverRate)
                {
                    var (child1, child2) = Crossover(population[index1], population[index2]);
                    if(newPopulation.Count < config.PopulationSize) newPopulation.Add(child1);
                    if(newPopulation.Count < config.PopulationSize) newPopulation.Add(child2);
                }
                else
                {
                    if(newPopulation.Count < config.PopulationSize) newPopulation.Add(population[index1]);
                    if(newPopulation.Count < config.PopulationSize) newPopulation.Add(population[index2]);
                }
            }

            foreach(var individual in newPopulation.ToList())
            {
                if(Random.NextDouble() < config.MutationRate)
                {
                    var mutationPoint = Random.Next(individual.Length);
                    individual[mutationPoint] += (Random.NextDouble() - 0.5) * 2 * individual[mutationPoint];
                }
            }

            newPopulation = newPopulation.Take(config.PopulationSize).ToList();
        }

        var finalFitnessScores = population.Select(individual => points.Sum(point => System.Math.Abs(f(point.Item1, individual) - point.Item2))).ToList();
        var bestIndex = finalFitnessScores.IndexOf(finalFitnessScores.Min());

        return population[bestIndex];
    }

    private int TournamentSelection(List<double> fitnessScores)
    {
        var index1 = Random.Next(fitnessScores.Count);
        var index2 = Random.Next(fitnessScores.Count);
        return fitnessScores[index1] < fitnessScores[index2] ? index1 : index2;
    }

    private (double[], double[]) Crossover(double[] parent1, double[] parent2)
    {
        var point = Random.Next(parent1.Length);
        var child1 = new double[parent1.Length];
        var child2 = new double[parent2.Length];
        for(var i = 0; i < parent1.Length; i++)
        {
            child1[i] = i <= point ? parent1[i] : parent2[i];
            child2[i] = i <= point ? parent2[i] : parent1[i];
        }
        return (child1, child2);
    }
}
