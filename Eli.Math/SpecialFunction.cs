namespace Eli.Math;

public class SpecialFunction
{
    public static double Gaussian(double x, double maxHeight, double mean, double standardDeviation) => maxHeight * System.Math.Exp(-System.Math.Pow( (x - mean) / standardDeviation, 2) / 2);
    
    public static double Laplace(double x, double maxHeight, double centerLocation, double diversity) => maxHeight * System.Math.Exp(-System.Math.Abs((x - centerLocation) / diversity));
}
