using Eli.DataStructures;
using NUnit.Framework;

namespace Eli.Test.DataStructures;

[TestFixture]
public class TrieTest
{
    [Test]
    public void InsertSingleWordShouldBeFound()
    {
        var trie = new Trie();
        trie.Insert("hello");
        Assert.IsNotNull(trie.Search("hello"));
    }

    [Test]
    public void InsertMultipleWordsShouldBeFound()
    {
        var trie = new Trie();
        trie.Insert("hello");
        trie.Insert("world");

        Assert.IsNotNull(trie.Search("hello"));
        Assert.IsNotNull(trie.Search("world"));
    }

    [Test]
    public void InsertWordPrefixShouldNotBeFoundAsWholeWord()
    {
        var trie = new Trie();
        trie.Insert("hello");
        Assert.IsNull(trie.Search("hell"));
    }

    [Test]
    public void SearchNonExistentWordShouldReturnFalse()
    {
        var trie = new Trie();
        trie.Insert("hello");
        Assert.IsNull(trie.Search("world"));
    }

    [Test]
    public void StartsWithPrefixShouldReturnTrue()
    {
        var trie = new Trie();
        trie.Insert("hello");

        Assert.IsNotNull(trie.StartsWith("hell"));
        Assert.IsNotNull(trie.StartsWith("he"));
        Assert.IsNotNull(trie.StartsWith("h"));
    }

    [Test]
    public void StartsWithNonExistentPrefixShouldReturnFalse()
    {
        var trie = new Trie();
        trie.Insert("hello");

        Assert.IsFalse(trie.StartsWith("world"));
        Assert.IsFalse(trie.StartsWith("w"));
    }
}
