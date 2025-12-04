using Eli.DataStructures;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Eli.Test.DataStructures;

[TestFixture]
public class FenwickTreeTest
{
    [Test]
    public void UpdateSingleValueShouldUpdateCorrectly()
    {
        var fenwickTree = new FenwickTree<int>(10);
        fenwickTree.Update(0, 5);
        ClassicAssert.AreEqual(5, fenwickTree.Query(1));
    }

    [Test]
    public void UpdateMultipleValuesShouldUpdateCorrectly()
    {
        var fenwickTree = new FenwickTree<int>(10);
        fenwickTree.Update(0, 5);
        fenwickTree.Update(1, 3);
        fenwickTree.Update(2, 7);

        ClassicAssert.AreEqual(5, fenwickTree.Query(0));
        ClassicAssert.AreEqual(8, fenwickTree.Query(1));
        ClassicAssert.AreEqual(15, fenwickTree.Query(2));
    }

    [Test]
    public void QueryRangeShouldReturnCorrectSum()
    {
        var fenwickTree = new FenwickTree<int>(10);
        fenwickTree.Update(0, 5);
        fenwickTree.Update(1, 3);
        fenwickTree.Update(2, 7);
        fenwickTree.Update(3, 6);

        ClassicAssert.AreEqual(8, fenwickTree.QueryRange(0, 1));
        ClassicAssert.AreEqual(15, fenwickTree.QueryRange(0, 2));
        ClassicAssert.AreEqual(16, fenwickTree.QueryRange(1, 3));
    }

    [Test]
    public void ConstructorWithValuesShouldInitializeCorrectly()
    {
        var values = new int[] { 1, 2, 3, 4, 5 };
        var fenwickTree = new FenwickTree<int>(values);

        ClassicAssert.AreEqual(1, fenwickTree.Query(0));
        ClassicAssert.AreEqual(3, fenwickTree.Query(1));
        ClassicAssert.AreEqual(6, fenwickTree.Query(2));
        ClassicAssert.AreEqual(10, fenwickTree.Query(3));
        ClassicAssert.AreEqual(15, fenwickTree.Query(4));
    }

    [Test]
    public void QueryRangeShouldReturnZeroWhenRangeIsEmpty()
    {
        var fenwickTree = new FenwickTree<int>(10);

        ClassicAssert.AreEqual(0, fenwickTree.QueryRange(0, 0));
        ClassicAssert.AreEqual(0, fenwickTree.QueryRange(1, 1));
    }
}
