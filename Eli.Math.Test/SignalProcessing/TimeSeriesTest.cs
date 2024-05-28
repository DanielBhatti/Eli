using Eli.Math.SignalProcessing;

namespace Eli.Math.Test.SignalProcessing;

[TestFixture]
public class TimeSeriesTests
{
    [Test]
    public void CrossCorrelationWithIdenticalSeriesReturnsPeakAtZeroLag()
    {
        var series = new double[] { 1, 2, 3, 4, 5 };
        var expected = new double[] { 1 };
        var result = TimeSeries.CrossCorrelation(series, series, 0, 0);

        Assert.That(result.Length, Is.EqualTo(expected.Length));
        Assert.That(result[0], Is.EqualTo(expected[0]).Within(1E-5));
    }

    [Test]
    public void CrossCorrelationWithLagReturnsCorrectLagValue()
    {
        var series1 = new double[] { 1, 2, 3, 4, 5 };
        var series2 = new double[] { 0, 1, 2, 3, 4 };
        var minLag = -1;
        var maxLag = 1;

        var expectedLength = 3;

        var result = TimeSeries.CrossCorrelation(series1, series2, minLag, maxLag);

        Assert.That(result.Length, Is.EqualTo(expectedLength));
        Assert.That(result[0], Is.EqualTo(0.5).Within(1E-5));
        Assert.That(result[1], Is.EqualTo(1.0).Within(1E-5));
    }

    [Test]
    public void CrossCorrelationWithEmptySeriesReturnsEmptyArray()
    {
        var series1 = new double[] { };
        var series2 = new double[] { };
        var minLag = -1;
        var maxLag = 1;

        var result = TimeSeries.CrossCorrelation(series1, series2, minLag, maxLag);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void CrossCorrelationWithConstantSeriesReturnsNaN()
    {
        var series1 = new double[] { 1, 1, 1, 1, 1 };
        var series2 = new double[] { 1, 1, 1, 1, 1 };
        var minLag = -1;
        var maxLag = 1;

        var result = TimeSeries.CrossCorrelation(series1, series2, minLag, maxLag);

        Assert.That(double.IsNaN(result[0]), Is.True);
        Assert.That(double.IsNaN(result[1]), Is.True);
        Assert.That(double.IsNaN(result[2]), Is.True);
    }
}
