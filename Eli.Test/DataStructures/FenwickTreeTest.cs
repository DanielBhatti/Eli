using Eli.DataStructures;
using NUnit.Framework;

namespace Eli.Test.DataStructures;

[TestFixture]
public class FenwickTreeTest
{
    [Test]
    public void UpdateSingleValueShouldUpdateCorrectly()
    {
        var fenwickTree = new FenwickTree<int>(10);
        fenwickTree.Update(0, 5);
        Assert.AreEqual(5, fenwickTree.Query(1));
    }

    [Test]
    public void UpdateMultipleValuesShouldUpdateCorrectly()
    {
        var fenwickTree = new FenwickTree<int>(10);
        fenwickTree.Update(0, 5);
        fenwickTree.Update(1, 3);
        fenwickTree.Update(2, 7);

        Assert.AreEqual(5, fenwickTree.Query(0));
        Assert.AreEqual(8, fenwickTree.Query(1));
        Assert.AreEqual(15, fenwickTree.Query(2));
    }

    [Test]
    public void QueryRangeShouldReturnCorrectSum()
    {
        var fenwickTree = new FenwickTree<int>(10);
        fenwickTree.Update(0, 5);
        fenwickTree.Update(1, 3);
        fenwickTree.Update(2, 7);
        fenwickTree.Update(3, 6);

        Assert.AreEqual(8, fenwickTree.QueryRange(0, 1));
        Assert.AreEqual(15, fenwickTree.QueryRange(0, 2));
        Assert.AreEqual(16, fenwickTree.QueryRange(1, 3));
    }

    [Test]
    public void ConstructorWithValuesShouldInitializeCorrectly()
    {
        var values = new int[] { 1, 2, 3, 4, 5 };
        var fenwickTree = new FenwickTree<int>(values);

        Assert.AreEqual(1, fenwickTree.Query(0));
        Assert.AreEqual(3, fenwickTree.Query(1));
        Assert.AreEqual(6, fenwickTree.Query(2));
        Assert.AreEqual(10, fenwickTree.Query(3));
        Assert.AreEqual(15, fenwickTree.Query(4));
    }

    [Test]
    public void QueryRangeShouldReturnZeroWhenRangeIsEmpty()
    {
        var fenwickTree = new FenwickTree<int>(10);

        Assert.AreEqual(0, fenwickTree.QueryRange(0, 0));
        Assert.AreEqual(0, fenwickTree.QueryRange(1, 1));
    }
}
