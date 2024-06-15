namespace Eli.Math;

public class SpecialFunction
{
    public static double Linear(double x, double slope, double yOffset) => yOffset + slope * x;

    public static double Quadratic(double x, double a, double b, double c) => a * System.Math.Pow(x, 2) + b * x + c;

    public static double Cubic(double x, double a, double b, double c, double d) => a * System.Math.Pow(x, 3) + b * System.Math.Pow(x, 2) + c * x + d;

    public static double Quartic(double x, double a, double b, double c, double d, double e) => a * System.Math.Pow(x, 4) + b * System.Math.Pow(x, 3) + c * System.Math.Pow(x, 2) + d * x + e;

    public static double Quintic(double x, double a, double b, double c, double d, double e, double f) => a * System.Math.Pow(x, 5) + b * System.Math.Pow(x, 4) + c * System.Math.Pow(x, 3) + d * System.Math.Pow(x, 2) + e * x + f;

    public static double Gaussian(double x, double maxHeight, double mean, double standardDeviation) => maxHeight * System.Math.Exp(-System.Math.Pow((x - mean) / standardDeviation, 2) / 2);

    public static double LogNormal(double x, double maxHeight, double mean, double standardDeviation, double xShift)
    {
        if(x <= xShift + 0.1) return 0.0;
        var logX = System.Math.Log(x - xShift);
        var exponent = -System.Math.Pow(logX - mean, 2) / (2 * standardDeviation * standardDeviation);
        var denominator = (x - xShift) * standardDeviation * System.Math.Sqrt(2 * System.Math.PI);
        return maxHeight * System.Math.Exp(exponent) / denominator;
    }

    public static double Laplacian(double x, double maxHeight, double centerLocation, double diversity) => maxHeight * System.Math.Exp(-System.Math.Abs((x - centerLocation) / diversity));
}
