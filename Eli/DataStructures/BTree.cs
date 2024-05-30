﻿using System;
using System.Collections.Generic;

namespace Eli.DataStructures;

public class BTreeNode<T1, T2> where T2 : IComparable<T2>
{
    public List<T1> Values { get; } = new();
    public List<T2> Keys { get; } = new();
    public List<BTreeNode<T1, T2>> Children { get; } = new();
    public bool IsLeaf => Children.Count == 0;
}

public class BTree<T1, T2> where T2 : IComparable<T2>
{
    private int MinimumDegree { get; init; }
    private BTreeNode<T1, T2> Root { get; set; }
    private Func<T1, T2> KeySelector { get; set; }

    public BTree(int minimumDegree, Func<T1, T2> keySelector)
    {
        if(minimumDegree <= 0) throw new ArgumentException("Minimum degree must be a positive integer", nameof(minimumDegree));
        MinimumDegree = minimumDegree;
        Root = new BTreeNode<T1, T2>();
        KeySelector = keySelector;
    }

    public void Insert(T1 value)
    {
        var key = KeySelector(value);
        var root = Root;
        if(root.Keys.Count == 2 * MinimumDegree - 1)
        {
            var newNode = new BTreeNode<T1, T2>();
            newNode.Children.Add(root);
            SplitChild(newNode, 0);
            Root = newNode;
        }
        InsertNonFull(Root, value, key);
    }

    public void Delete(T1 value)
    {
        var key = KeySelector(value);
        Delete(Root, key);
        if(Root.Keys.Count == 0 && !Root.IsLeaf) Root = Root.Children[0];
    }

    public bool Search(T1 value) => Search(Root, KeySelector(value)) != null;

    private void SplitChild(BTreeNode<T1, T2> node, int index)
    {
        var child = node.Children[index];
        var newNode = new BTreeNode<T1, T2>();

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

    private void InsertNonFull(BTreeNode<T1, T2> node, T1 value, T2 key)
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

    private BTreeNode<T1, T2> Search(BTreeNode<T1, T2> node, T2 key)
    {
        var i = 0;
        while(i < node.Keys.Count && key.CompareTo(node.Keys[i]) > 0) i++;
        if(i < node.Keys.Count && key.CompareTo(node.Keys[i]) == 0) return node;
        return node.IsLeaf ? null : Search(node.Children[i], key);
    }

    private void Delete(BTreeNode<T1, T2> node, T2 key)
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
                    var predKey = GetPredecessor(predChild);
                    node.Keys[idx] = predKey;
                    var predValue = node.Values[idx];
                    node.Values[idx] = predValue;
                    Delete(predChild, predKey);
                }
                else if(succChild.Keys.Count >= MinimumDegree)
                {
                    var succKey = GetSuccessor(succChild);
                    node.Keys[idx] = succKey;
                    var succValue = node.Values[idx];
                    node.Values[idx] = succValue;
                    Delete(succChild, succKey);
                }
                else
                {
                    MergeChildren(node, idx);
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

    private T2 GetPredecessor(BTreeNode<T1, T2> node)
    {
        while(!node.IsLeaf) node = node.Children[node.Keys.Count];
        return node.Keys[^1];
    }

    private T2 GetSuccessor(BTreeNode<T1, T2> node)
    {
        while(!node.IsLeaf) node = node.Children[0];
        return node.Keys[0];
    }

    private void MergeChildren(BTreeNode<T1, T2> node, int idx)
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

    private void FixChildSize(BTreeNode<T1, T2> node, int idx)
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
            if(leftSibling != null) MergeChildren(node, idx - 1);
            else if(rightSibling != null) MergeChildren(node, idx);
        }
    }
}