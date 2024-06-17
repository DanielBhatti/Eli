using System.Numerics;

namespace Eli.Math.MonteCarlo;

public class RandomNumberGenerator
{
    private Random Random { get; }
    public int Seed { get; }

    public RandomNumberGenerator(int seed)
    {
        Seed = seed;
        Random = new Random(seed);
    }

    public T GetUniform<T>() where T : INumber<T> => GenerateRandomNumber<T>();

    public T GetGaussian<T>(T mean, T stdDev) where T : INumber<T>
    {
        var u1 = 1.0 - Random.NextDouble();
        var u2 = 1.0 - Random.NextDouble();
        var randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2);
        var randNormal = Convert.ToDouble(mean) + Convert.ToDouble(stdDev) * randStdNormal;

        return (T)Convert.ChangeType(randNormal, typeof(T));
    }

    public T GetPoisson<T>(T lambda) where T : INumber<T>
    {
        var k = 0;
        var p = 1.0;
        var l = System.Math.Exp(-Convert.ToDouble(lambda));

        do
        {
            k++;
            p *= Random.NextDouble();
        } while(p > l);

        return (T)Convert.ChangeType(k - 1, typeof(T));
    }

    public T GetExponential<T>(T lambda) where T : INumber<T>
    {
        var randExponential = -System.Math.Log(1.0 - Random.NextDouble()) / Convert.ToDouble(lambda);

        return (T)Convert.ChangeType(randExponential, typeof(T));
    }

    public T GetBinomial<T>(int n, T p) where T : INumber<T>
    {
        var successes = 0;
        for(var i = 0; i < n; i++) if((T)(object)Random.NextDouble() < p) successes++;
        return (T)Convert.ChangeType(successes, typeof(T));
    }

    public T GenerateRandomNumber<T>() where T : INumber<T>
    {
        if(typeof(T) == typeof(int)) return (T)(object)Random.Next();
        else if(typeof(T) == typeof(double)) return (T)(object)Random.NextDouble();
        else if(typeof(T) == typeof(float)) return (T)(object)(float)Random.NextDouble();
        else if(typeof(T) == typeof(decimal)) return (T)(object)(decimal)Random.NextDouble();
        else throw new NotSupportedException($"The type {typeof(T)} is not supported for uniform distribution.");
    }
}
