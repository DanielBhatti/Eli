using Eli.Math.MonteCarlo;
using NUnit.Framework;
using System;
using System.Numerics;

namespace Eli.Test.Math.MonteCarlo;

[TestFixture]
public class RandomNumberGeneratorTest
{
    private const int Seed = 42;
    private RandomNumberGenerator RandomNumberGenerator { get; set; }

    [SetUp]
    public void Setup() => RandomNumberGenerator = new RandomNumberGenerator(Seed);

    [Test]
    public void TestGaussianDistribution()
    {
        var mean = 0.0;
        var stdDev = 1.0;
        var value = RandomNumberGenerator.GetGaussian(mean, stdDev);
        Assert.That(value, Is.Not.NaN);
    }

    [Test]
    public void TestGaussianDistributionWithParams()
    {
        var mean = 1.0;
        var stdDev = 0.5;
        var value = RandomNumberGenerator.GetGaussian(mean, stdDev);
        Assert.That(value, Is.Not.NaN);
    }

    [Test]
    public void TestPoissonDistribution()
    {
        var lambda = 4.0;
        var value = RandomNumberGenerator.GetPoisson(lambda);
        Assert.That(value, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void TestExponentialDistribution()
    {
        var lambda = 1.0;
        var value = RandomNumberGenerator.GetExponential(lambda);
        Assert.That(value, Is.GreaterThanOrEqualTo(0.0));
    }

    [Test]
    public void TestExponentialDistributionWithParams()
    {
        var lambda = 2.0;
        var value = RandomNumberGenerator.GetExponential(lambda);
        Assert.That(value, Is.GreaterThanOrEqualTo(0.0));
    }

    [Test]
    public void TestBinomialDistribution()
    {
        var n = 10;
        var p = 0.5;
        var value = RandomNumberGenerator.GetBinomial(n, p);
        Assert.That(value, Is.InRange(0, n));
    }

    [Test]
    public void TestBinomialDistributionWithParams()
    {
        var n = 20;
        var p = 0.7;
        var value = RandomNumberGenerator.GetBinomial(n, p);
        Assert.That(value, Is.InRange(0, n));
    }
}
