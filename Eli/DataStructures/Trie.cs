using System.Collections.Generic;

namespace Eli.DataStructures;

public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; } = new Dictionary<char, TrieNode>();
    public bool IsEndOfWord { get; set; }
}

public class Trie : Tree<string, string>
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

    public string? Search(string key)
    {
        var current = Root;
        foreach(var c in key)
        {
            if(!current.Children.ContainsKey(c)) return null;
            current = current.Children[c];
        }
        return current.IsEndOfWord ? key : null;
    }

    public void Delete(string key) => Delete(Root, key, 0);

    private bool Delete(TrieNode current, string word, int index)
    {
        if(index == word.Length)
        {
            if(!current.IsEndOfWord) return false;
            current.IsEndOfWord = false;
            return current.Children.Count == 0;
        }

        var c = word[index];
        if(!current.Children.ContainsKey(c)) return false;

        var shouldDeleteCurrentNode = Delete(current.Children[c], word, index + 1);

        if(shouldDeleteCurrentNode)
        {
            _ = current.Children.Remove(c);
            return current.Children.Count == 0 && !current.IsEndOfWord;
        }
        return false;
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