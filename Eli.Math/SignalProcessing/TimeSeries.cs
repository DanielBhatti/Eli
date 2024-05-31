namespace Eli.Math.SignalProcessing;

public static class TimeSeries
{
    public static double[] CrossCorrelation(double[] series1, double[] series2, int minLag, int maxLag)
    {
        if(series1.Length == 0 || series2.Length == 0) return new double[] { };

        var n = series1.Length;
        var ccf = new double[maxLag - minLag + 1];
        var lagIndex = 0;

        var series1Mean = Mean(series1);
        var series2Mean = Mean(series2);
        var series1StdDev = StdDev(series1);
        var series2StdDev = StdDev(series2);
        for(var lag = minLag; lag <= maxLag; lag++)
        {
            var sum = 0.0;
            var count = 0;
            for(var i = 0; i < n; i++)
            {
                var j = i + lag;
                if(j >= 0 && j < n)
                {
                    sum += (series1[i] - series1Mean) * (series2[j] - series2Mean);
                    count++;
                }
            }
            ccf[lagIndex++] = sum / (count * series1StdDev * series2StdDev);
        }

        return ccf;
    }

    private static double Mean(double[] series) => series.Sum() / series.Length;

    private static double StdDev(double[] series)
    {
        var mean = Mean(series);
        var sumSquares = 0.0;
        foreach(var value in series) sumSquares += System.Math.Pow(value - mean, 2);
        return System.Math.Sqrt(sumSquares / series.Length);
    }
}
