using Eli.Math.Probability;

namespace Eli.Math.Test.Probability;

[TestFixture]
public class ProbabilitySeriesTests
{
    [Test]
    public void ExpectedAverage_WithValidData_ReturnsCorrectResult()
    {
        var data = new List<(double Probability, double Value)>
        {
            (0.5, 2),
            (0.25, 4),
            (0.25, 6)
        };
        var expected = 3.5;
        var result = ProbabilitySeries.ExpectedValue(data);
        Assert.That(result, Is.EqualTo(expected).Within(0.0001), "Expected average calculation is incorrect.");
    }

    [Test]
    public void Variance_WithValidData_ReturnsCorrectResult()
    {
        var data = new List<(double Probability, double Value)>
        {
            (0.5, 2),
            (0.25, 4),
            (0.25, 6)
        };
        var expected = 2.75;
        var result = ProbabilitySeries.Variance(data);
        Assert.That(result, Is.EqualTo(expected).Within(0.0001), "Variance calculation is incorrect.");
    }

    [Test]
    public void ExpectedAverage_WithEmptyData_ReturnsZero()
    {
        var data = new List<(double Probability, double Value)>();
        var result = ProbabilitySeries.ExpectedValue(data);
        Assert.That(result, Is.EqualTo(0), "Expected average should be zero for empty data.");
    }

    [Test]
    public void Variance_WithEmptyData_ReturnsZero()
    {
        var data = new List<(double Probability, double Value)>();
        var result = ProbabilitySeries.Variance(data);
        Assert.That(result, Is.EqualTo(0), "Variance should be zero for empty data.");
    }
}
