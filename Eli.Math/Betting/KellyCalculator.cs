using Eli.Math.Arithmetic;
using System.Numerics;

namespace Eli.Math.Betting;

public static class KellyCalculator
{
    public static double CalculateKelly(IEnumerable<(double Probability, double Value)> events, double kellyFraction) => events.Sum(e => e.Probability * System.Math.Log(1.0 + kellyFraction * e.Value));

    public static T CalculateKellyFractionQuadraticApproximation<T>(IEnumerable<(T Probability, T Value)> events) where T : INumber<T>
    {
        var numerator = events.Select(e => e.Probability * e.Value).KahanSum();
        var denominator = events.Select(e => e.Probability * e.Value * e.Value).KahanSum();
        return denominator == T.Zero ? T.Zero : numerator / denominator;
    }
}
