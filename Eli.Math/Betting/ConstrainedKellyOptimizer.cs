public static class ConstrainedKellyOptimizer
{
    public static Dictionary<string, decimal> Optimize(
        Dictionary<string, List<decimal>> priceSeries,
        Dictionary<string, (string baseTicker, decimal ratio)>? leverageLinks = null,
        decimal maxLeverage = 1.0m,
        bool noShorting = true)
    {
        var tickers = priceSeries.Keys.ToList();
        var n = tickers.Count;
        var m = priceSeries.First().Value.Count - 1;

        var returns = new double[m, n];
        for(var i = 0; i < m; i++)
        {
            for(var j = 0; j < n; j++)
            {
                var p0 = priceSeries[tickers[j]][i];
                var p1 = priceSeries[tickers[j]][i + 1];
                returns[i, j] = Math.Log((double)(p1 / p0));
            }
        }

        var mu = new double[n];
        for(var j = 0; j < n; j++)
        {
            double sum = 0;
            for(var i = 0; i < m; i++)
                sum += returns[i, j];
            mu[j] = sum / m;
        }

        var cov = new double[n, n];
        for(var a = 0; a < n; a++)
        {
            for(var b = 0; b < n; b++)
            {
                double sum = 0;
                for(var i = 0; i < m; i++)
                    sum += (returns[i, a] - mu[a]) * (returns[i, b] - mu[b]);
                cov[a, b] = sum / (m - 1);
            }
        }

        var invCov = InvertSymmetricMatrix(cov);
        var f = Multiply(invCov, mu);

        if(leverageLinks != null)
        {
            foreach(var (leveraged, (baseTicker, ratio)) in leverageLinks)
            {
                var i = tickers.IndexOf(leveraged);
                var j = tickers.IndexOf(baseTicker);
                if(i == -1 || j == -1)
                    continue;

                f[i] = (double)ratio * f[j];
            }
        }

        if(noShorting)
        {
            for(var i = 0; i < n; i++)
                f[i] = Math.Max(f[i], 0);
        }

        var sumF = f.Sum();
        if((decimal)sumF > maxLeverage)
        {
            for(var i = 0; i < n; i++)
                f[i] = f[i] * (double)maxLeverage / sumF;
        }

        var result = new Dictionary<string, decimal>();
        for(var i = 0; i < n; i++)
            result[tickers[i]] = (decimal)f[i];

        return result;
    }

    private static double[] Multiply(double[,] matrix, double[] vector)
    {
        var rows = matrix.GetLength(0);
        var cols = matrix.GetLength(1);
        var result = new double[rows];
        for(var i = 0; i < rows; i++)
        {
            double sum = 0;
            for(var j = 0; j < cols; j++)
                sum += matrix[i, j] * vector[j];
            result[i] = sum;
        }
        return result;
    }

    private static double[,] InvertSymmetricMatrix(double[,] mat)
    {
        var n = mat.GetLength(0);
        var a = new double[n, n];
        var inv = new double[n, n];

        for(var i = 0; i < n; i++)
        {
            for(var j = 0; j < n; j++)
            {
                a[i, j] = mat[i, j];
                inv[i, j] = (i == j) ? 1.0 : 0.0;
            }
        }

        for(var i = 0; i < n; i++)
        {
            var diag = a[i, i];
            if(Math.Abs(diag) < 1e-10)
                throw new InvalidOperationException("Matrix is singular");

            for(var j = 0; j < n; j++)
            {
                a[i, j] /= diag;
                inv[i, j] /= diag;
            }

            for(var k = 0; k < n; k++)
            {
                if(k == i) continue;
                var factor = a[k, i];
                for(var j = 0; j < n; j++)
                {
                    a[k, j] -= factor * a[i, j];
                    inv[k, j] -= factor * inv[i, j];
                }
            }
        }

        return inv;
    }
}
