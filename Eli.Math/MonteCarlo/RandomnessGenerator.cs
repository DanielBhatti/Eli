namespace Eli.Math.MonteCarlo;

public class RandomnessGenerator
{
    private Random Random { get; }
    public int Seed { get; }

    public RandomnessGenerator(int seed)
    {
        Seed = seed;
        Random = new Random(seed);
    }

    public DateTime GetRandomDate(DateTime startDate, DateTime endDate, bool setHours = true, bool setMinutes = true, bool setSeconds = true)
    {
        var range = (endDate - startDate).Days + 1;
        var randomDate = startDate.AddDays(Random.Next(range));

        var hours = setHours ? Random.Next(0, 24) : 0;
        var minutes = setMinutes ? Random.Next(0, 60) : 0;
        var seconds = setSeconds ? Random.Next(0, 60) : 0;
        return randomDate.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
    }

    public double GetUniform(double start, double end) => start + (end - start) * Random.NextDouble();

    public double GetGaussian(double mean, double stdDev)
    {
        var u1 = 1.0 - Random.NextDouble();
        var u2 = 1.0 - Random.NextDouble();
        var randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2);
        var randNormal = mean + stdDev * randStdNormal;

        return randNormal;
    }

    public double GetPoisson(double lambda)
    {
        var k = 0;
        var p = 1.0;
        var l = System.Math.Exp(-lambda);

        do
        {
            k++;
            p *= Random.NextDouble();
        } while(p > l);

        return k - 1;
    }

    public double GetExponential(double lambda)
    {
        var randExponential = -System.Math.Log(1.0 - Random.NextDouble()) / lambda;
        return randExponential;
    }

    public double GetBinomial(int n, double p)
    {
        var successes = 0;
        for(var i = 0; i < n; i++) if(Random.NextDouble() < p) successes++;
        return successes;
    }
}
