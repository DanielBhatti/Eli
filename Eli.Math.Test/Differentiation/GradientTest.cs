using Eli.Math.Differentiation;

namespace Eli.Math.Test.Differentiation;

[TestFixture]
public class GradientTests
{
    private const double Epsilon = 1e-6;

    [Test]
    public void Compute_LinearFunction_ReturnsConstantGradient()
    {
        static double linear(double[] variables) => variables[0] + variables[1];
        double[] inputs = { 3.0, 4.0 };

        var gradient = Gradient.Compute(linear, inputs);

        Assert.That(gradient[0], Is.EqualTo(1.0).Within(Epsilon));
        Assert.That(gradient[1], Is.EqualTo(1.0).Within(Epsilon));
    }

    [Test]
    public void Compute_QuadraticFunction_ReturnsCorrectGradient()
    {
        static double quadratic(double[] variables) => variables[0] * variables[0] + variables[1] * variables[1];
        double[] inputs = { 2.0, 3.0 };

        var gradient = Gradient.Compute(quadratic, inputs);

        Assert.That(gradient[0], Is.EqualTo(4.0).Within(Epsilon));
        Assert.That(gradient[1], Is.EqualTo(6.0).Within(Epsilon));
    }

    [Test]
    public void Compute_MixedFunction_ReturnsCorrectGradient()
    {
        static double mixed(double[] variables) => variables[0] * variables[0] + 3 * variables[0] * variables[1];
        double[] inputs = { 2.0, 3.0 };

        var gradient = Gradient.Compute(mixed, inputs);

        Assert.That(gradient[0], Is.EqualTo(2 * inputs[0] + 3 * inputs[1]).Within(Epsilon));
        Assert.That(gradient[1], Is.EqualTo(3 * inputs[0]).Within(Epsilon));
    }
}
