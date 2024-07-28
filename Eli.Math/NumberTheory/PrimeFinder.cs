using System.Numerics;

namespace Eli.Math.NumberTheory;

public class PrimeFinder
{
    public static bool IsPrime(BigInteger n, int k = 5)
    {
        if(n <= 1) return false;
        if(n <= 3) return true;
        if(n % 2 == 0 || n % 3 == 0) return false;

        var d = n - 1;
        var r = 0;

        while(d % 2 == 0)
        {
            d /= 2;
            r++;
        }

        for(var i = 0; i < k; i++) if(!MillerRabinTest(n, d, r)) return false;

        return true;
    }

    private static bool MillerRabinTest(BigInteger n, BigInteger d, int r)
    {
        var a = 2 + (BigInteger)r * (n - 4);
        var x = BigInteger.ModPow(a, d, n);

        if(x == 1 || x == n - 1) return true;

        for(var i = 0; i < r - 1; i++)
        {
            x = BigInteger.ModPow(x, 2, n);
            if(x == n - 1) return true;
        }
        return false;
    }

    public static BigInteger FindNextPrime(BigInteger n)
    {
        if(n <= 1) return 2;

        var primeCandidate = n + 1;
        if(primeCandidate % 2 == 0) primeCandidate++;
        while(!IsPrime(primeCandidate)) primeCandidate += 2;

        return primeCandidate;
    }
}