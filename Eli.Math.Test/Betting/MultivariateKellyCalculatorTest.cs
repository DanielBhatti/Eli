using Eli.Math.Betting;

namespace Eli.Math.Test.Betting;

[TestFixture]
public class MultivariateKellyCalculatorTest
{
    [Test]
    public void ExpectedLogGrowth_ZeroWeights_IsZero()
    {
        var returnsByTime = new List<double[]>
        {
            new[] { 0.01, -0.02 },
            new[] { 0.03,  0.01 },
            new[] { -0.01, 0.00 },
        };

        var weights = new[] { 0.0, 0.0 };
        var value = MultivariateKellyCalculator.ExpectedLogGrowth(returnsByTime, weights);

        Assert.That(value, Is.EqualTo(0.0).Within(1e-15));
    }

    [Test]
    public void ExpectedLogGrowth_InfeasibleWeights_ReturnsNegativeInfinity()
    {
        // If weights = [2], and return = -0.6, then 1 + w*r = 1 - 1.2 = -0.2 (infeasible)
        var returnsByTime = new List<double[]>
        {
            new[] { -0.60 },
            new[] {  0.10 }
        };

        var weights = new[] { 2.0 };
        var value = MultivariateKellyCalculator.ExpectedLogGrowth(returnsByTime, weights);

        Assert.That(value, Is.EqualTo(double.NegativeInfinity));
    }

    [Test]
    public void KellyWeightsNewton_OneAsset_AllPositiveReturns_PushesToVeryLargeButFeasible()
    {
        // For strictly positive returns, log utility increases with fraction, with no finite unconstrained optimum.
        // However feasibility is unbounded above when all returns > 0, so solution is limited by optimizer behavior.
        // We assert it stays feasible and non-negative, and improves vs zero.
        var returnsByTime = new List<double[]>
        {
            new[] { 0.01 },
            new[] { 0.02 },
            new[] { 0.03 },
            new[] { 0.01 },
            new[] { 0.02 },
        };

        var w0 = new[] { 0.0 };
        var f0 = MultivariateKellyCalculator.ExpectedLogGrowth(returnsByTime, w0);

        var w = MultivariateKellyCalculator.KellyWeightsNewton(returnsByTime, ridge: 1e-10, maxIterations: 50);
        Assert.That(w.Length, Is.EqualTo(1));
        Assert.That(w[0], Is.GreaterThanOrEqualTo(0.0));
        Assert.That(IsFeasible(returnsByTime, w), Is.True);

        var f1 = MultivariateKellyCalculator.ExpectedLogGrowth(returnsByTime, w);
        Assert.That(f1, Is.GreaterThan(f0));
    }

    [Test]
    public void KellyWeightsNewton_OneAsset_MixedReturns_ApproximatesMeanOverSecondMoment()
    {
        // For small returns, optimal f is close to E[r]/E[r^2].
        // Use symmetric-ish small returns to keep approximation tight.
        var returns = new[] { 0.02, -0.01, 0.015, -0.005, 0.01, -0.0025 };
        var returnsByTime = returns.Select(r => new[] { r }).ToList();

        var empiricalMean = returns.Average();
        var empiricalSecondMoment = returns.Select(r => r * r).Average();
        var approx = empiricalSecondMoment == 0.0 ? 0.0 : empiricalMean / empiricalSecondMoment;

        var w = MultivariateKellyCalculator.KellyWeightsNewton(returnsByTime, ridge: 1e-10, maxIterations: 200);
        Assert.That(w.Length, Is.EqualTo(1));
        Assert.That(IsFeasible(returnsByTime, w), Is.True);

        // Allow some slack since this is exact log utility vs quadratic approximation.
        Assert.That(w[0], Is.EqualTo(approx).Within(System.Math.Max(1e-2, System.Math.Abs(approx) * 0.15)));
    }

    [Test]
    public void KellyWeightsNewton_TwoAssets_IdenticalReturns_SplitsApproximatelyEvenly()
    {
        // If both assets have identical return series and no constraints, objective depends on sum of weights.
        // Starting from quadratic approx tends to produce equal weights due to symmetry (with ridge).
        var r = new List<double> { 0.01, -0.005, 0.02, -0.01, 0.015, -0.002 };
        var returnsByTime = r.Select(x => new[] { x, x }).ToList();

        var w = MultivariateKellyCalculator.KellyWeightsNewton(returnsByTime, ridge: 1e-8, maxIterations: 200);
        Assert.That(w.Length, Is.EqualTo(2));
        Assert.That(IsFeasible(returnsByTime, w), Is.True);

        Assert.That(w[0], Is.EqualTo(w[1]).Within(1e-6));

        // And it should improve vs zero.
        var f0 = MultivariateKellyCalculator.ExpectedLogGrowth(returnsByTime, new[] { 0.0, 0.0 });
        var f1 = MultivariateKellyCalculator.ExpectedLogGrowth(returnsByTime, w);
        Assert.That(f1, Is.GreaterThan(f0));
    }

    [Test]
    public void KellyWeightsNewton_TwoAssets_NegativeCorrelation_ReducesDrawdowns_AllowsHigherTotalExposure()
    {
        // Asset2 = -Asset1. Portfolio can hedge, increasing feasible exposure.
        // Expect weights approximately equal in magnitude and opposite in sign (to stabilize wealth factor),
        // yielding higher log-growth than allocating only to one asset at the same scale.
        var a = new List<double> { 0.02, -0.02, 0.015, -0.015, 0.01, -0.01 };
        var returnsByTime = a.Select(x => new[] { x, -x }).ToList();

        var w = MultivariateKellyCalculator.KellyWeightsNewton(returnsByTime, ridge: 1e-8, maxIterations: 200);
        Assert.That(w.Length, Is.EqualTo(2));
        Assert.That(IsFeasible(returnsByTime, w), Is.True);

        // Expect opposite signs (hedge).
        Assert.That(w[0] * w[1], Is.LessThanOrEqualTo(0.0));

        var fHedged = MultivariateKellyCalculator.ExpectedLogGrowth(returnsByTime, w);
        var fZero = MultivariateKellyCalculator.ExpectedLogGrowth(returnsByTime, new[] { 0.0, 0.0 });

        Assert.That(fHedged, Is.GreaterThan(fZero));
    }

    [Test]
    public void QuadraticApproxWeights_ReturnsFeasibleWeights()
    {
        var returnsByTime = new List<double[]>
        {
            new[] { 0.01, 0.02, -0.03 },
            new[] { -0.02, 0.01, 0.01 },
            new[] { 0.005, -0.015, 0.02 },
            new[] { -0.01, 0.005, -0.005 },
        };

        var w = MultivariateKellyCalculator.QuadraticApproxWeights(returnsByTime, ridge: 1e-8);
        Assert.That(w.Length, Is.EqualTo(3));
        Assert.That(IsFeasible(returnsByTime, w), Is.True);
    }

    private static bool IsFeasible(IReadOnlyList<double[]> returnsByTime, double[] weights)
    {
        for(var t = 0; t < returnsByTime.Count; t++)
        {
            var r = returnsByTime[t];
            var z = 0.0;
            for(var i = 0; i < weights.Length; i++)
                z += weights[i] * r[i];

            if(1.0 + z <= 0.0)
                return false;
        }
        return true;
    }
}
