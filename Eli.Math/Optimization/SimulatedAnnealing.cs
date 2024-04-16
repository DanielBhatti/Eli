using Eli.Math.Optimization.Config;

namespace Eli.Math.Optimization;

public class SimulatedAnnealing : OptimizationAlgorithm<SimulatedAnnealingConfig>
{
    public SimulatedAnnealing(SimulatedAnnealingConfig config) : base(config) { }

    public override double[] Optimize(IEnumerable<(double, double)> points, Func<double, double[], double> f, double[] functionParameters)
    {
        var random = new Random();
        var temperature = Config.InitialTemperature;
        var currentSolution = new double[functionParameters.Length];
        Array.Copy(functionParameters, currentSolution, functionParameters.Length);

        var currentCost = f(currentSolution[0], currentSolution);
        var bestSolution = new double[functionParameters.Length];
        Array.Copy(currentSolution, bestSolution, functionParameters.Length);
        var bestCost = currentCost;

        for(var i = 0; i < Config.MaxIterations; i++)
        {
            var parameterIndex = random.Next(currentSolution.Length);
            var oldValue = currentSolution[parameterIndex];
            var newValue = oldValue + temperature * (random.NextDouble() * 2 - 1);

            currentSolution[parameterIndex] = newValue;
            var newCost = f(currentSolution[0], currentSolution);

            if(newCost < currentCost || random.NextDouble() < System.Math.Exp((currentCost - newCost) / temperature))
            {
                currentCost = newCost;
                if(currentCost < bestCost)
                {
                    Array.Copy(currentSolution, bestSolution, functionParameters.Length);
                    bestCost = currentCost;
                }
            }
            else currentSolution[parameterIndex] = oldValue;
            temperature *= Config.CoolingRate;
        }
        return bestSolution;
    }
}
