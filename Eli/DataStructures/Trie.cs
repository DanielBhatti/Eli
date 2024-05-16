using System.Collections.Generic;

namespace Eli.DataStructures;

public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
    public bool IsEndOfWord { get; set; }
}

public class Trie
{
    private TrieNode Root { get; }

    public Trie() => Root = new TrieNode();

    public void Insert(string word)
    {
        var current = Root;
        foreach(var c in word)
        {
            if(!current.Children.ContainsKey(c)) current.Children[c] = new TrieNode();
            current = current.Children[c];
        }
        current.IsEndOfWord = true;
    }

    public bool Search(string word)
    {
        var current = Root;
        foreach(var c in word)
        {
            if(!current.Children.ContainsKey(c)) return false;
            current = current.Children[c];
        }
        return current.IsEndOfWord;
    }

    public bool StartsWith(string prefix)
    {
        var current = Root;
        foreach(var c in prefix)
        {
            if(!current.Children.ContainsKey(c)) return false;
            current = current.Children[c];
        }
        return true;
    }
}