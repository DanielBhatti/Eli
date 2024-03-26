namespace Eli.Math.Differentiation;

public static class Derivative
{
    public static double Differentiate(Func<double, double> f, double x, double delta = 1e-6) => Differentiate(f, delta)(x);

    public static Func<double, double> Differentiate(Func<double, double> f, double delta = 1e-6) => x => (f(x + delta) - f(x - delta)) / (2 * delta);
}
