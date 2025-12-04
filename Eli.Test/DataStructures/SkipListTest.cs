using Eli.DataStuctures;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Eli.Test.DataStructures;

[TestFixture]
public class SkipListTest
{
    [Test]
    public void Insert_SingleValue_ShouldBeFound()
    {
        var skipList = new SkipList<int>(0.5, 16, 42); // Fixed seed for reproducibility
        skipList.Insert(10);
        ClassicAssert.IsTrue(skipList.Search(10));
    }

    [Test]
    public void Insert_MultipleValues_ShouldBeFound()
    {
        var skipList = new SkipList<int>(0.5, 16, 42); // Fixed seed for reproducibility
        skipList.Insert(10);
        skipList.Insert(20);
        skipList.Insert(5);

        ClassicAssert.IsTrue(skipList.Search(10));
        ClassicAssert.IsTrue(skipList.Search(20));
        ClassicAssert.IsTrue(skipList.Search(5));
    }

    [Test]
    public void Search_NonExistentValue_ShouldReturnFalse()
    {
        var skipList = new SkipList<int>(0.5, 16, 42); // Fixed seed for reproducibility
        skipList.Insert(10);
        ClassicAssert.IsFalse(skipList.Search(20));
    }

    [Test]
    public void Insert_DuplicateValues_ShouldNotAffectSearch()
    {
        var skipList = new SkipList<int>(0.5, 16, 42); // Fixed seed for reproducibility
        skipList.Insert(10);
        skipList.Insert(10);
        ClassicAssert.IsTrue(skipList.Search(10));
    }

    [Test]
    public void Insert_Values_ShouldMaintainCorrectOrder()
    {
        var skipList = new SkipList<int>(0.5, 16, 42); // Fixed seed for reproducibility
        skipList.Insert(10);
        skipList.Insert(5);
        skipList.Insert(15);

        ClassicAssert.IsTrue(skipList.Search(5));
        ClassicAssert.IsTrue(skipList.Search(10));
        ClassicAssert.IsTrue(skipList.Search(15));
    }
}
