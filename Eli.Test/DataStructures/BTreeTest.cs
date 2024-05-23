using Eli.DataStructures;
using NUnit.Framework;

namespace Eli.Test.DataStructures;

[TestFixture]
public class BTreeTests
{
    [Test]
    public void InsertSingleKeyShouldBeFound()
    {
        var bTree = new BTree<int, int>(10, i => i);
        bTree.Insert(10);
        Assert.IsTrue(bTree.Search(10));
    }

    [Test]
    public void InsertMultipleKeysShouldBeFound()
    {
        var bTree = new BTree<int, int>(2, i => i); 
        bTree.Insert(10);
        bTree.Insert(20);
        bTree.Insert(5);

        Assert.IsTrue(bTree.Search(10));
        Assert.IsTrue(bTree.Search(20));
        Assert.IsTrue(bTree.Search(5));
    }

    [Test]
    public void DeleteSingleKeyShouldNotBeFound()
    {
        var bTree = new BTree<int, int>(20, i => i); 
        bTree.Insert(10);
        bTree.Delete(10);
        Assert.IsFalse(bTree.Search(10));
    }

    [Test]
    public void DeleteKeyFromLeafShouldNotBeFound()
    {
        var bTree = new BTree<int, int>(5, i => i); 
        bTree.Insert(10);
        bTree.Insert(20);
        bTree.Insert(5);
        bTree.Delete(20);

        Assert.IsTrue(bTree.Search(10));
        Assert.IsFalse(bTree.Search(20));
        Assert.IsTrue(bTree.Search(5));
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

        Assert.IsTrue(bTree.Search(10));
        Assert.IsFalse(bTree.Search(20));
        Assert.IsTrue(bTree.Search(5));
        Assert.IsTrue(bTree.Search(15));
        Assert.IsTrue(bTree.Search(25));
    }
}