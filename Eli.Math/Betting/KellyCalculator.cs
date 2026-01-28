using Eli.Math.Arithmetic;
using System.Numerics;

namespace Eli.Math.Betting;

public static class KellyCalculator
{
    public static double CalculateKelly(
        IEnumerable<(double Probability, double Value)> events,
        double kellyFraction) =>
        events.Sum(e => e.Probability * System.Math.Log(1.0 + kellyFraction * e.Value));

    public static T CalculateKellyFractionQuadraticApproximation<T>(
        IEnumerable<(T Probability, T Value)> events)
        where T : INumber<T>
    {
        var expectedReturn = events
            .Select(e => e.Probability * e.Value)
            .KahanSum();

        var expectedSquaredReturn = events
            .Select(e => e.Probability * e.Value * e.Value)
            .KahanSum();

        return expectedSquaredReturn == T.Zero
            ? T.Zero
            : expectedReturn / expectedSquaredReturn;
    }

    public static double ExpectedLogGrowth(
        IEnumerable<(double Probability, double ReturnPerUnit)> outcomes,
        double fraction)
    {
        var expectedLogGrowth = 0.0;

        foreach(var (probability, returnPerUnit) in outcomes)
        {
            if(probability < 0.0 || double.IsNaN(probability) || double.IsInfinity(probability))
                return double.NegativeInfinity;

            var growthFactor = 1.0 + fraction * returnPerUnit;
            if(growthFactor <= 0.0 || double.IsNaN(growthFactor) || double.IsInfinity(growthFactor))
                return double.NegativeInfinity;

            expectedLogGrowth += probability * System.Math.Log(growthFactor);
        }

        return expectedLogGrowth;
    }

    public static double ExpectedLogGrowthDerivative(
        IEnumerable<(double Probability, double ReturnPerUnit)> outcomes,
        double fraction)
    {
        var derivative = 0.0;

        foreach(var (probability, returnPerUnit) in outcomes)
        {
            var denominator = 1.0 + fraction * returnPerUnit;
            if(denominator <= 0.0 || double.IsNaN(denominator) || double.IsInfinity(denominator))
                return double.NaN;

            derivative += probability * (returnPerUnit / denominator);
        }

        return derivative;
    }

    public static double KellyFractionQuadraticApprox(
        IEnumerable<(double Probability, double ReturnPerUnit)> outcomes)
    {
        var expectedReturn = 0.0;
        var expectedSquaredReturn = 0.0;

        foreach(var (probability, returnPerUnit) in outcomes)
        {
            expectedReturn += probability * returnPerUnit;
            expectedSquaredReturn += probability * returnPerUnit * returnPerUnit;
        }

        return expectedSquaredReturn == 0.0
            ? 0.0
            : expectedReturn / expectedSquaredReturn;
    }

    public static (double LowerBound, double UpperBound) FeasibleFractionDomain(
        IEnumerable<(double Probability, double ReturnPerUnit)> outcomes,
        double minFraction,
        double maxFraction,
        double safetyEpsilon = 1e-12)
    {
        var lowerBound = double.NegativeInfinity;
        var upperBound = double.PositiveInfinity;

        foreach(var (_, returnPerUnit) in outcomes)
        {
            if(returnPerUnit > 0.0)
                lowerBound = System.Math.Max(lowerBound, -1.0 / returnPerUnit + safetyEpsilon);
            else if(returnPerUnit < 0.0)
                upperBound = System.Math.Min(upperBound, -1.0 / returnPerUnit - safetyEpsilon);
        }

        lowerBound = System.Math.Max(lowerBound, minFraction);
        upperBound = System.Math.Min(upperBound, maxFraction);

        if(double.IsNaN(lowerBound) || double.IsNaN(upperBound) || lowerBound >= upperBound)
            return (double.NaN, double.NaN);

        return (lowerBound, upperBound);
    }

    public static double KellyFraction1D(
        IEnumerable<(double Probability, double ReturnPerUnit)> outcomes,
        double minFraction = -10.0,
        double maxFraction = 10.0,
        int maxIterations = 200,
        double tolerance = 1e-12)
    {
        var outcomeList = outcomes as IList<(double Probability, double ReturnPerUnit)>
                          ?? outcomes.ToList();

        if(outcomeList.Count == 0)
            return 0.0;

        var (lowerBound, upperBound) =
            FeasibleFractionDomain(outcomeList, minFraction, maxFraction);

        if(double.IsNaN(lowerBound) || double.IsNaN(upperBound))
            return 0.0;

        var derivativeAtLower = ExpectedLogGrowthDerivative(outcomeList, lowerBound);
        var derivativeAtUpper = ExpectedLogGrowthDerivative(outcomeList, upperBound);

        if(double.IsNaN(derivativeAtLower) || double.IsNaN(derivativeAtUpper))
            return 0.0;

        if(derivativeAtLower <= 0.0)
            return lowerBound;

        if(derivativeAtUpper >= 0.0)
            return upperBound;

        const double goldenRatioConjugate = 0.6180339887498948482;

        var left = lowerBound;
        var right = upperBound;

        var midLeft = right - goldenRatioConjugate * (right - left);
        var midRight = left + goldenRatioConjugate * (right - left);

        var growthAtMidLeft = ExpectedLogGrowth(outcomeList, midLeft);
        var growthAtMidRight = ExpectedLogGrowth(outcomeList, midRight);

        for(var i = 0;
            i < maxIterations &&
            (right - left) > tolerance * (1.0 + System.Math.Abs(left) + System.Math.Abs(right));
            i++)
        {
            if(growthAtMidLeft < growthAtMidRight)
            {
                left = midLeft;
                midLeft = midRight;
                growthAtMidLeft = growthAtMidRight;

                midRight = left + goldenRatioConjugate * (right - left);
                growthAtMidRight = ExpectedLogGrowth(outcomeList, midRight);
            }
            else
            {
                right = midRight;
                midRight = midLeft;
                growthAtMidRight = growthAtMidLeft;

                midLeft = right - goldenRatioConjugate * (right - left);
                growthAtMidLeft = ExpectedLogGrowth(outcomeList, midLeft);
            }
        }

        var optimalFraction = (left + right) / 2.0;

        if(optimalFraction <= lowerBound) return lowerBound;
        if(optimalFraction >= upperBound) return upperBound;

        return optimalFraction;
    }

    public static IEnumerable<(double Probability, double ReturnPerUnit)>
        FromEmpiricalOutcomes(IEnumerable<double> samples)
    {
        var sampleList = samples as IList<double> ?? samples.ToList();
        var sampleCount = sampleList.Count;

        if(sampleCount == 0)
            yield break;

        var probability = 1.0 / sampleCount;

        foreach(var returnPerUnit in sampleList)
            yield return (probability, returnPerUnit);
    }

    public static IEnumerable<double> SimpleReturnsFromPrices(IEnumerable<double> prices)
    {
        using var enumerator = prices.GetEnumerator();
        if(!enumerator.MoveNext())
            yield break;

        var previousPrice = enumerator.Current;

        while(enumerator.MoveNext())
        {
            var currentPrice = enumerator.Current;

            if(previousPrice > 0.0 && currentPrice > 0.0)
                yield return currentPrice / previousPrice - 1.0;

            previousPrice = currentPrice;
        }
    }
}
