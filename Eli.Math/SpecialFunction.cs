namespace Eli.Math;

public class SpecialFunction
{
    public static double Linear(double x, double slope, double yOffset) => yOffset + slope * x;

    public static double Quadratic(double x, double a, double b, double c) => a * System.Math.Pow(x, 2) + b * x + c;

    public static double Quintic(double x, double a, double b, double c, double d) => a * System.Math.Pow(x, 3) + b * System.Math.Pow(x, 2) + c * x + d;

    public static double Gaussian(double x, double maxHeight, double mean, double standardDeviation) => maxHeight * System.Math.Exp(-System.Math.Pow( (x - mean) / standardDeviation, 2) / 2);

    public static double Laplacian(double x, double maxHeight, double centerLocation, double diversity) => maxHeight * System.Math.Exp(-System.Math.Abs((x - centerLocation) / diversity));
}
