using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Eli.DataStructures;

public class FenwickTree<T> where T : INumber<T>
{
    private T[] Tree { get; }

    public FenwickTree(int size) => Tree = new T[size];

    public FenwickTree(IEnumerable<T> values)
    {
        var valueArray = values.ToArray();
        Tree = new T[valueArray.Length];
        for(var index = 0; index < valueArray.Length; index++) Update(index, valueArray[index]);
    }

    public void Update(int index, T delta)
    {
        for(var i = index; i < Tree.Length; i |= i + 1) Tree[i] += delta;
    }

    public T Query(int index)
    {
        var sum = T.Zero;
        for(var i = index; i >= 0; i = (i & (i + 1)) - 1) sum += Tree[i];
        return sum;
    }

    public T QueryRange(int left, int right) => Query(right) - (left > 0 ? Query(left - 1) : T.Zero);
}
