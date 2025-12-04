using Eli.DataStructures;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Eli.Test.DataStructures;

[TestFixture]
public class TrieTest
{
    [Test]
    public void InsertSingleWordShouldBeFound()
    {
        var trie = new Trie();
        trie.Insert("hello");
        ClassicAssert.IsNotNull(trie.Search("hello"));
    }

    [Test]
    public void InsertMultipleWordsShouldBeFound()
    {
        var trie = new Trie();
        trie.Insert("hello");
        trie.Insert("world");

        ClassicAssert.IsNotNull(trie.Search("hello"));
        ClassicAssert.IsNotNull(trie.Search("world"));
    }

    [Test]
    public void InsertWordPrefixShouldNotBeFoundAsWholeWord()
    {
        var trie = new Trie();
        trie.Insert("hello");
        ClassicAssert.IsNull(trie.Search("hell"));
    }

    [Test]
    public void SearchNonExistentWordShouldReturnFalse()
    {
        var trie = new Trie();
        trie.Insert("hello");
        ClassicAssert.IsNull(trie.Search("world"));
    }

    [Test]
    public void StartsWithPrefixShouldReturnTrue()
    {
        var trie = new Trie();
        trie.Insert("hello");

        ClassicAssert.IsNotNull(trie.StartsWith("hell"));
        ClassicAssert.IsNotNull(trie.StartsWith("he"));
        ClassicAssert.IsNotNull(trie.StartsWith("h"));
    }

    [Test]
    public void StartsWithNonExistentPrefixShouldReturnFalse()
    {
        var trie = new Trie();
        trie.Insert("hello");

        ClassicAssert.IsFalse(trie.StartsWith("world"));
        ClassicAssert.IsFalse(trie.StartsWith("w"));
    }
}
