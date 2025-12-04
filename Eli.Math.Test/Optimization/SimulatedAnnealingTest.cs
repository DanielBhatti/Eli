using Eli.Math.Optimization;
using Eli.Math.Optimization.Config;

namespace Eli.Math.Test.Optimization;

[TestFixture]
public class SimulatedAnnealingTest
{
    private SimulatedAnnealing SimulatedAnnealing { get; set; }
    private Func<double, double, double, double> TestFunction1 => (x, p1, p2) => System.Math.Pow(p1, 2) + System.Math.Pow(x - p2, 2);

    [SetUp]
    public void SetUp()
    {
        var config = new SimulatedAnnealingConfig()
        {
            InitialTemperature = 100.0,
            CoolingRate = 0.99,
            MaxIterations = 1000,
        };
        SimulatedAnnealing = new SimulatedAnnealing(config);
    }

    [Test]
    public void TestOptimizeFindsMinimumNearOrigin()
    {
        var result = SimulatedAnnealing.Optimize(new List<(double, double)>(), TestFunction1, -3.0, 0.0);
        Assert.That(result.Item1, Is.EqualTo(0.0).Within(0.1), "Optimization did not converge near the expected minimum.");
    }

    [Test]
    public void TestOptimizeReducesFunctionValue()
    {
        var initialCost = TestFunction1(0.0, -3.0, 0.0);
        var result = SimulatedAnnealing.Optimize(new List<(double, double)>(), TestFunction1, -3.0, 0.0);
        var resultCost = TestFunction1(0.0, result.Item1, result.Item2);

        Assert.That(initialCost, Is.GreaterThanOrEqualTo(resultCost), "Optimization did not reduce the function value.");
    }

    [Test]
    public void TestOptimizationWithCooling()
    {
        var result = SimulatedAnnealing.Optimize(new List<(double, double)>(), TestFunction1, -3.0, 0.0);
        Assert.That(result.Item1, Is.EqualTo(0.0).Within(1.0), "Optimization with aggressive cooling did not perform as expected.");
    }
}
