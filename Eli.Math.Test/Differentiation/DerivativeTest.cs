using Eli.Math.Differentiation;

namespace Eli.Math.Test.Differentiation;

[TestFixture]
public class DerivativeTests
{
    private const double Epsilon = 1e-6;

    [Test]
    public void Differentiate_LinearFunction_ReturnsOne()
    {
        static double linear(double x) => x;
        var pointToDifferentiate = 5.0;

        var derivativeFunc = Derivative.Differentiate(linear);
        var derivativeAtPoint = derivativeFunc(pointToDifferentiate);

        Assert.That(derivativeAtPoint, Is.EqualTo(1.0).Within(Epsilon));
    }

    [Test]
    public void Differentiate_QuadraticFunction_ReturnsCorrectDerivative()
    {
        static double quadratic(double x) => x * x;
        var pointToDifferentiate = 3.0;

        var derivativeFunc = Derivative.Differentiate(quadratic);
        var derivativeAtPoint = derivativeFunc(pointToDifferentiate);

        Assert.That(derivativeAtPoint, Is.EqualTo(6.0).Within(Epsilon));
    }

    [Test]
    public void Differentiate_SinFunction_ReturnsCos()
    {
        var pointToDifferentiate = System.Math.PI / 4;

        var derivativeFunc = Derivative.Differentiate(System.Math.Sin);
        var derivativeAtPoint = derivativeFunc(pointToDifferentiate);

        Assert.That(derivativeAtPoint, Is.EqualTo(System.Math.Cos(pointToDifferentiate)).Within(Epsilon));
    }
}
