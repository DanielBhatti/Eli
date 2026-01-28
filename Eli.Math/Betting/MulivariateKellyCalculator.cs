namespace Eli.Math.Betting;

public static class MultivariateKellyCalculator
{
    public static double ExpectedLogGrowth(IReadOnlyList<double[]> returnsByTime, double[] weights)
    {
        var tCount = returnsByTime.Count;
        if(tCount == 0) return 0.0;

        var sum = 0.0;
        for(var t = 0; t < tCount; t++)
        {
            var r = returnsByTime[t];
            var z = Dot(weights, r);
            var growth = 1.0 + z;
            if(growth <= 0.0 || double.IsNaN(growth) || double.IsInfinity(growth))
                return double.NegativeInfinity;
            sum += System.Math.Log(growth);
        }
        return sum / tCount;
    }

    public static double[] KellyWeightsNewton(
        IReadOnlyList<double[]> returnsByTime,
        double ridge = 1e-10,
        int maxIterations = 100,
        double gradientTolerance = 1e-10,
        double stepTolerance = 1e-12)
    {
        if(returnsByTime.Count == 0) return Array.Empty<double>();

        var d = returnsByTime[0].Length;
        if(d == 0) return Array.Empty<double>();
        for(var t = 1; t < returnsByTime.Count; t++)
            if(returnsByTime[t].Length != d)
                throw new ArgumentException("All return vectors must have the same dimension.");

        // Initial guess: quadratic approximation w0 = (E[rr^T] + ridge I)^(-1) E[r]
        var (mu, secondMoment) = EstimateMoments(returnsByTime);
        var w = SolveSymmetricPositiveDefinite(AddRidge(secondMoment, ridge), mu);

        // If start is infeasible, scale down until feasible.
        w = MakeFeasibleByScaling(returnsByTime, w);

        var current = ExpectedLogGrowth(returnsByTime, w);

        var grad = new double[d];
        var hess = new double[d, d];

        for(var iter = 0; iter < maxIterations; iter++)
        {
            ComputeGradientAndHessian(returnsByTime, w, grad, hess);

            var gradNormInf = InfinityNorm(grad);
            if(gradNormInf < gradientTolerance)
                return w;

            // Newton step: (-H) is PSD; we solve (-H + ridge I) step = grad to ascend concave objective
            // because H is negative semidefinite.
            var negH = Negate(hess);
            AddRidgeInPlace(negH, ridge);

            var step = SolveSymmetricPositiveDefinite(negH, grad);

            if(InfinityNorm(step) < stepTolerance)
                return w;

            // Backtracking line search with feasibility and improvement checks
            var alpha = 1.0;
            const double c1 = 1e-4;

            while(alpha > 1e-16)
            {
                var candidate = AddScaled(w, step, alpha);

                // Feasibility check
                if(!IsFeasible(returnsByTime, candidate))
                {
                    alpha *= 0.5;
                    continue;
                }

                var candidateValue = ExpectedLogGrowth(returnsByTime, candidate);

                // Armijo condition for ascent: f(w + a p) >= f(w) + c1 * a * grad^T p
                var armijoRhs = current + c1 * alpha * Dot(grad, step);
                if(candidateValue >= armijoRhs)
                {
                    w = candidate;
                    current = candidateValue;
                    break;
                }

                alpha *= 0.5;
            }

            if(alpha <= 1e-16)
                return w; // cannot make progress while staying feasible
        }

        return w;
    }

    public static double[] QuadraticApproxWeights(IReadOnlyList<double[]> returnsByTime, double ridge = 1e-10)
    {
        var (mu, secondMoment) = EstimateMoments(returnsByTime);
        var w = SolveSymmetricPositiveDefinite(AddRidge(secondMoment, ridge), mu);
        return MakeFeasibleByScaling(returnsByTime, w);
    }

    private static void ComputeGradientAndHessian(
        IReadOnlyList<double[]> returnsByTime,
        double[] weights,
        double[] gradientOut,
        double[,] hessianOut)
    {
        var tCount = returnsByTime.Count;
        var d = weights.Length;

        Array.Clear(gradientOut, 0, d);
        Array.Clear(hessianOut, 0, hessianOut.Length);

        for(var t = 0; t < tCount; t++)
        {
            var r = returnsByTime[t];
            var denom = 1.0 + Dot(weights, r);

            // Caller ensures feasibility; still guard numeric issues.
            if(denom <= 0.0 || double.IsNaN(denom) || double.IsInfinity(denom))
                throw new InvalidOperationException("Encountered infeasible point during derivative computation.");

            var inv = 1.0 / denom;
            var inv2 = inv * inv;

            for(var i = 0; i < d; i++)
                gradientOut[i] += r[i] * inv;

            // Hessian = -E[ r r^T / (1+w·r)^2 ]
            for(var i = 0; i < d; i++)
            {
                var ri = r[i];
                for(var j = 0; j <= i; j++)
                {
                    hessianOut[i, j] -= ri * r[j] * inv2;
                }
            }
        }

        var scale = 1.0 / tCount;
        for(var i = 0; i < d; i++)
        {
            gradientOut[i] *= scale;
            for(var j = 0; j <= i; j++)
            {
                hessianOut[i, j] *= scale;
                hessianOut[j, i] = hessianOut[i, j];
            }
        }
    }

    private static (double[] mu, double[,] secondMoment) EstimateMoments(IReadOnlyList<double[]> returnsByTime)
    {
        var tCount = returnsByTime.Count;
        var d = returnsByTime[0].Length;

        var mu = new double[d];
        var m2 = new double[d, d];

        for(var t = 0; t < tCount; t++)
        {
            var r = returnsByTime[t];
            for(var i = 0; i < d; i++)
                mu[i] += r[i];

            for(var i = 0; i < d; i++)
            {
                var ri = r[i];
                for(var j = 0; j <= i; j++)
                    m2[i, j] += ri * r[j];
            }
        }

        var invT = 1.0 / tCount;
        for(var i = 0; i < d; i++)
            mu[i] *= invT;

        for(var i = 0; i < d; i++)
            for(var j = 0; j <= i; j++)
            {
                m2[i, j] *= invT;
                m2[j, i] = m2[i, j];
            }

        return (mu, m2);
    }

    private static bool IsFeasible(IReadOnlyList<double[]> returnsByTime, double[] weights)
    {
        for(var t = 0; t < returnsByTime.Count; t++)
            if(1.0 + Dot(weights, returnsByTime[t]) <= 0.0)
                return false;
        return true;
    }

    private static double[] MakeFeasibleByScaling(IReadOnlyList<double[]> returnsByTime, double[] weights)
    {
        if(IsFeasible(returnsByTime, weights))
            return weights;

        var scaled = (double[])weights.Clone();
        var scale = 1.0;
        while(scale > 1e-16)
        {
            scale *= 0.5;
            for(var i = 0; i < scaled.Length; i++)
                scaled[i] = weights[i] * scale;
            if(IsFeasible(returnsByTime, scaled))
                return scaled;
        }

        // Fall back to zero vector
        Array.Clear(scaled, 0, scaled.Length);
        return scaled;
    }

    private static double Dot(double[] a, double[] b)
    {
        var s = 0.0;
        for(var i = 0; i < a.Length; i++) s += a[i] * b[i];
        return s;
    }

    private static double[] AddScaled(double[] w, double[] step, double alpha)
    {
        var x = new double[w.Length];
        for(var i = 0; i < w.Length; i++)
            x[i] = w[i] + alpha * step[i];
        return x;
    }

    private static double InfinityNorm(double[] v)
    {
        var m = 0.0;
        for(var i = 0; i < v.Length; i++)
            m = System.Math.Max(m, System.Math.Abs(v[i]));
        return m;
    }

    private static double[,] AddRidge(double[,] a, double ridge)
    {
        var d = a.GetLength(0);
        var b = (double[,])a.Clone();
        for(var i = 0; i < d; i++) b[i, i] += ridge;
        return b;
    }

    private static void AddRidgeInPlace(double[,] a, double ridge)
    {
        var d = a.GetLength(0);
        for(var i = 0; i < d; i++) a[i, i] += ridge;
    }

    private static double[,] Negate(double[,] a)
    {
        var d0 = a.GetLength(0);
        var d1 = a.GetLength(1);
        var b = new double[d0, d1];
        for(var i = 0; i < d0; i++)
            for(var j = 0; j < d1; j++)
                b[i, j] = -a[i, j];
        return b;
    }

    // Simple Cholesky solver for SPD matrices
    private static double[] SolveSymmetricPositiveDefinite(double[,] a, double[] b)
    {
        var n = b.Length;
        var l = new double[n, n];

        for(var i = 0; i < n; i++)
        {
            for(var j = 0; j <= i; j++)
            {
                var sum = a[i, j];
                for(var k = 0; k < j; k++)
                    sum -= l[i, k] * l[j, k];

                if(i == j)
                {
                    if(sum <= 0.0 || double.IsNaN(sum) || double.IsInfinity(sum))
                        throw new InvalidOperationException("Matrix not positive definite (consider increasing ridge).");
                    l[i, j] = System.Math.Sqrt(sum);
                }
                else
                {
                    l[i, j] = sum / l[j, j];
                }
            }
        }

        // Forward solve L y = b
        var y = new double[n];
        for(var i = 0; i < n; i++)
        {
            var sum = b[i];
            for(var k = 0; k < i; k++)
                sum -= l[i, k] * y[k];
            y[i] = sum / l[i, i];
        }

        // Backward solve L^T x = y
        var x = new double[n];
        for(var i = n - 1; i >= 0; i--)
        {
            var sum = y[i];
            for(var k = i + 1; k < n; k++)
                sum -= l[k, i] * x[k];
            x[i] = sum / l[i, i];
        }

        return x;
    }
}
