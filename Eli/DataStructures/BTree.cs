using System;
using System.Collections.Generic;

namespace Eli.DataStructures;

internal class BTreeNode<TKey, TValue> where TKey : IComparable<TKey>
{
    public List<TValue> Values { get; } = [];
    public List<TKey> Keys { get; } = [];
    public List<BTreeNode<TKey, TValue>> Children { get; } = [];
    public bool IsLeaf => Children.Count == 0;
}

public class BTree<TKey, TValue> : Tree<TKey, TValue> where TKey : IComparable<TKey>
{
    private int MinimumDegree { get; init; }
    private BTreeNode<TKey, TValue> Root { get; set; }
    private Func<TValue, TKey> KeySelector { get; set; }

    public BTree(int minimumDegree, Func<TValue, TKey> keySelector)
    {
        if(minimumDegree <= 0) throw new ArgumentException("Minimum degree must be a positive integer", nameof(minimumDegree));
        MinimumDegree = minimumDegree;
        Root = new BTreeNode<TKey, TValue>();
        KeySelector = keySelector;
    }

    public void Insert(TValue value)
    {
        var key = KeySelector(value);
        var root = Root;
        if(root.Keys.Count == 2 * MinimumDegree - 1)
        {
            var newNode = new BTreeNode<TKey, TValue>();
            newNode.Children.Add(root);
            SplitChild(newNode, 0);
            Root = newNode;
        }
        InsertNonFull(Root, value, key);
    }

    public void Delete(TKey key)
    {
        Delete(Root, key);
        if(Root.Keys.Count == 0 && !Root.IsLeaf) Root = Root.Children[0];
    }

    public TValue? Search(TKey key) => BTree<TKey, TValue>.Search(Root, key);

    private void SplitChild(BTreeNode<TKey, TValue> node, int index)
    {
        var child = node.Children[index];
        var newNode = new BTreeNode<TKey, TValue>();

        newNode.Keys.AddRange(child.Keys.GetRange(MinimumDegree, MinimumDegree - 1));
        newNode.Values.AddRange(child.Values.GetRange(MinimumDegree, MinimumDegree - 1));
        child.Keys.RemoveRange(MinimumDegree, MinimumDegree - 1);
        child.Values.RemoveRange(MinimumDegree, MinimumDegree - 1);

        if(!child.IsLeaf)
        {
            newNode.Children.AddRange(child.Children.GetRange(MinimumDegree, MinimumDegree));
            child.Children.RemoveRange(MinimumDegree, MinimumDegree);
        }

        node.Children.Insert(index + 1, newNode);
        node.Keys.Insert(index, child.Keys[MinimumDegree - 1]);
        node.Values.Insert(index, child.Values[MinimumDegree - 1]);
        child.Keys.RemoveAt(MinimumDegree - 1);
        child.Values.RemoveAt(MinimumDegree - 1);
    }

    private void InsertNonFull(BTreeNode<TKey, TValue> node, TValue value, TKey key)
    {
        if(node.IsLeaf)
        {
            var insertIndex = ~node.Keys.BinarySearch(key);
            node.Keys.Insert(insertIndex, key);
            node.Values.Insert(insertIndex, value);
            return;
        }

        var i = node.Keys.BinarySearch(key);
        if(i >= 0) return;

        i = ~i;

        if(node.Children[i].Keys.Count == 2 * MinimumDegree - 1)
        {
            SplitChild(node, i);
            if(node.Keys[i].CompareTo(key) < 0) i++;
        }
        InsertNonFull(node.Children[i], value, key);
    }

    private static TValue? Search(BTreeNode<TKey, TValue> node, TKey key)
    {
        var i = 0;
        while(i < node.Keys.Count && key.CompareTo(node.Keys[i]) > 0) i++;
        if(i < node.Keys.Count && key.CompareTo(node.Keys[i]) == 0) return node.Values[i];
        return node.IsLeaf ? default : BTree<TKey, TValue>.Search(node.Children[i], key);
    }

    private void Delete(BTreeNode<TKey, TValue> node, TKey key)
    {
        var idx = node.Keys.BinarySearch(key);
        if(idx >= 0)
        {
            if(node.IsLeaf)
            {
                node.Keys.RemoveAt(idx);
                node.Values.RemoveAt(idx);
            }
            else
            {
                var predChild = node.Children[idx];
                var succChild = node.Children[idx + 1];

                if(predChild.Keys.Count >= MinimumDegree)
                {
                    var predKey = BTree<TKey, TValue>.GetPredecessor(predChild);
                    node.Keys[idx] = predKey;
                    var predValue = node.Values[idx];
                    node.Values[idx] = predValue;
                    Delete(predChild, predKey);
                }
                else if(succChild.Keys.Count >= MinimumDegree)
                {
                    var succKey = BTree<TKey, TValue>.GetSuccessor(succChild);
                    node.Keys[idx] = succKey;
                    var succValue = node.Values[idx];
                    node.Values[idx] = succValue;
                    Delete(succChild, succKey);
                }
                else
                {
                    BTree<TKey, TValue>.MergeChildren(node, idx);
                    Delete(predChild, key);
                }
            }
        }
        else if(!node.IsLeaf)
        {
            idx = ~idx;
            var child = node.Children[idx];
            if(child.Keys.Count == MinimumDegree - 1)
            {
                FixChildSize(node, idx);
            }
            Delete(node.Children[idx], key);
        }
    }

    private static TKey GetPredecessor(BTreeNode<TKey, TValue> node)
    {
        while(!node.IsLeaf) node = node.Children[node.Keys.Count];
        return node.Keys[^1];
    }

    private static TKey GetSuccessor(BTreeNode<TKey, TValue> node)
    {
        while(!node.IsLeaf) node = node.Children[0];
        return node.Keys[0];
    }

    private static void MergeChildren(BTreeNode<TKey, TValue> node, int idx)
    {
        var child = node.Children[idx];
        var sibling = node.Children[idx + 1];

        child.Keys.Add(node.Keys[idx]);
        child.Values.Add(node.Values[idx]);
        child.Keys.AddRange(sibling.Keys);
        child.Values.AddRange(sibling.Values);
        child.Children.AddRange(sibling.Children);

        node.Keys.RemoveAt(idx);
        node.Values.RemoveAt(idx);
        node.Children.RemoveAt(idx + 1);
    }

    private void FixChildSize(BTreeNode<TKey, TValue> node, int idx)
    {
        var child = node.Children[idx];
        var leftSibling = idx > 0 ? node.Children[idx - 1] : null;
        var rightSibling = idx < node.Children.Count - 1 ? node.Children[idx + 1] : null;

        if(leftSibling != null && leftSibling.Keys.Count >= MinimumDegree)
        {
            child.Keys.Insert(0, node.Keys[idx - 1]);
            child.Values.Insert(0, node.Values[idx - 1]);
            node.Keys[idx - 1] = leftSibling.Keys[^1];
            node.Values[idx - 1] = leftSibling.Values[^1];
            leftSibling.Keys.RemoveAt(leftSibling.Keys.Count - 1);
            leftSibling.Values.RemoveAt(leftSibling.Values.Count - 1);
            if(!leftSibling.IsLeaf)
            {
                child.Children.Insert(0, leftSibling.Children[^1]);
                leftSibling.Children.RemoveAt(leftSibling.Children.Count - 1);
            }
        }
        else if(rightSibling != null && rightSibling.Keys.Count >= MinimumDegree)
        {
            child.Keys.Add(node.Keys[idx]);
            child.Values.Add(node.Values[idx]);
            node.Keys[idx] = rightSibling.Keys[0];
            node.Values[idx] = rightSibling.Values[0];
            rightSibling.Keys.RemoveAt(0);
            rightSibling.Values.RemoveAt(0);
            if(!rightSibling.IsLeaf)
            {
                child.Children.Add(rightSibling.Children[0]);
                rightSibling.Children.RemoveAt(0);
            }
        }
        else
        {
            if(leftSibling != null) BTree<TKey, TValue>.MergeChildren(node, idx - 1);
            else if(rightSibling != null) BTree<TKey, TValue>.MergeChildren(node, idx);
        }
    }
}