using Eli.DataStructures;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Eli.Test.DataStructures;

[TestFixture]
public class BTreeTests
{
    [Test]
    public void InsertSingleKeyShouldBeFound()
    {
        var bTree = new BTree<int, int>(10, i => i);
        bTree.Insert(10);
        ClassicAssert.IsNotNull(bTree.Search(10));
    }

    [Test]
    public void InsertMultipleKeysShouldBeFound()
    {
        var bTree = new BTree<int, int>(2, i => i);
        bTree.Insert(10);
        bTree.Insert(20);
        bTree.Insert(5);

        ClassicAssert.IsNotNull(bTree.Search(10));
        ClassicAssert.IsNotNull(bTree.Search(20));
        ClassicAssert.IsNotNull(bTree.Search(5));
    }

    [Test]
    public void DeleteSingleKeyShouldNotBeFound()
    {
        var bTree = new BTree<int, int>(20, i => i);
        bTree.Insert(10);
        bTree.Delete(10);
        ClassicAssert.AreEqual(0, bTree.Search(10));
    }

    [Test]
    public void DeleteKeyFromLeafShouldNotBeFound()
    {
        var bTree = new BTree<int, int>(5, i => i);
        bTree.Insert(10);
        bTree.Insert(20);
        bTree.Insert(5);
        bTree.Delete(20);

        ClassicAssert.IsNotNull(bTree.Search(10));
        ClassicAssert.AreEqual(0, bTree.Search(20));
        ClassicAssert.IsNotNull(bTree.Search(5));
    }

    [Test]
    public void DeleteKeyFromNonLeafShouldNotBeFound()
    {
        var bTree = new BTree<int, int>(15, i => i);
        bTree.Insert(10);
        bTree.Insert(20);
        bTree.Insert(5);
        bTree.Insert(15);
        bTree.Insert(25);
        bTree.Delete(20);

        ClassicAssert.IsNotNull(bTree.Search(10));
        ClassicAssert.AreEqual(0, bTree.Search(20));
        ClassicAssert.IsNotNull(bTree.Search(5));
        ClassicAssert.IsNotNull(bTree.Search(15));
        ClassicAssert.IsNotNull(bTree.Search(25));
    }
}