using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Eli.DataStructures;

public  class BloomFilter<T>
{
    private int Size { get; }
    private BitArray BitArray { get; }
    private IEnumerable<HashAlgorithm> HashAlgorithms { get; }

    public BloomFilter(int size, int hashFunctionCount)
    {
        Size = size;
        BitArray = new BitArray(size);
        HashAlgorithms = Enumerable.Range(0, hashFunctionCount).Select(i => MD5.Create());
    }

    public void Add(T item)
    {
        foreach(var hashFunction in HashAlgorithms)
        {
            var hash = Math.Abs(BitConverter.ToInt32(hashFunction.ComputeHash(BitConverter.GetBytes(item.GetHashCode())), 0)) % Size;
            BitArray.Set(hash, true);
        }
    }

    public bool Contains(T item)
    {
        foreach(var hashFunction in HashAlgorithms)
        {
            var hash = Math.Abs(BitConverter.ToInt32(hashFunction.ComputeHash(BitConverter.GetBytes(item.GetHashCode())), 0)) % Size;
            if(!BitArray.Get(hash)) return false;
        }
        return true;
    }
}
