using Eli.DataStructures;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;

namespace Eli.Test.DataStructures;

[TestFixture]
public class BloomFilterTest
{
    [Test]
    public void Add_SingleItem_ShouldBeContained()
    {
        var bloomFilter = new BloomFilter<int>(100, 3);
        bloomFilter.Add(42);
        ClassicAssert.IsTrue(bloomFilter.Contains(42));
    }

    [Test]
    public void Add_MultipleItems_AllShouldBeContained()
    {
        var bloomFilter = new BloomFilter<int>(100, 3);
        var items = new List<int> { 42, 56, 89 };
        foreach(var item in items) bloomFilter.Add(item);
        foreach(var item in items) ClassicAssert.IsTrue(bloomFilter.Contains(item));
    }

    [Test]
    public void Contains_NonAddedItem_ShouldReturnFalse()
    {
        var bloomFilter = new BloomFilter<int>(100, 3);
        bloomFilter.Add(42);
        ClassicAssert.IsFalse(bloomFilter.Contains(56));
    }

    [Test]
    public void Contains_ItemAfterMultipleAdditions_ShouldReturnTrue()
    {
        var bloomFilter = new BloomFilter<int>(100, 3);
        bloomFilter.Add(42);
        bloomFilter.Add(56);
        bloomFilter.Add(42);

        ClassicAssert.IsTrue(bloomFilter.Contains(42));
        ClassicAssert.IsTrue(bloomFilter.Contains(56));
    }

    [Test]
    public void Contains_RandomItem_ShouldReturnFalse()
    {
        var bloomFilter = new BloomFilter<int>(100, 3);
        ClassicAssert.IsFalse(bloomFilter.Contains(999));
    }

    [Test]
    public void Add_Strings_ShouldBeContained()
    {
        var bloomFilter = new BloomFilter<string>(100, 3);
        var items = new List<string> { "hello", "world", "bloom", "filter" };
        foreach(var item in items) bloomFilter.Add(item);
        foreach(var item in items) ClassicAssert.IsTrue(bloomFilter.Contains(item));
    }
}
