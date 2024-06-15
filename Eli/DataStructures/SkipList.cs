using System;

namespace Eli.DataStuctures;

public class SkipListNode<T>(T value, int level) where T : notnull
{
    public T Value { get; set; } = value;
    public SkipListNode<T>[] Forward { get; set; } = new SkipListNode<T>[level + 1];
}

public class SkipList<T> where T : notnull, IComparable<T>
{
    private double Probability { get; }
    private int MaxLevel { get; }
    private SkipListNode<T> Head { get; }
    private Random Random { get; }
    private int Level { get; set; }

    public SkipList(double probability = 0.5, int maxLevel = 5, int? seed = null)
    {
        Probability = probability;
        MaxLevel = maxLevel;
        Head = new SkipListNode<T>(default!, MaxLevel);
        Random = seed is not null ? new Random(seed.Value) : new Random();
        Level = 0;
    }

    public void Insert(T value)
    {
        var update = new SkipListNode<T>[MaxLevel + 1];
        var current = Head;

        for(var i = Level; i >= 0; i--)
        {
            while(current.Forward[i] != null && current.Forward[i].Value.CompareTo(value) < 0) current = current.Forward[i];
            update[i] = current;
        }

        current = current.Forward[0];

        if(current == null || !current.Value.Equals(value))
        {
            var newLevel = RandomLevel();
            if(newLevel > Level)
            {
                for(var i = Level + 1; i <= newLevel; i++) update[i] = Head;
                Level = newLevel;
            }

            var newNode = new SkipListNode<T>(value, newLevel);
            for(var i = 0; i <= newLevel; i++)
            {
                newNode.Forward[i] = update[i].Forward[i];
                update[i].Forward[i] = newNode;
            }
        }
    }

    private int RandomLevel()
    {
        var newLevel = 0;
        while(Random.NextDouble() < Probability && newLevel < MaxLevel) newLevel++;
        return newLevel;
    }

    public bool Search(T value)
    {
        var current = Head;
        for(var i = Level; i >= 0; i--)
        {
            while(current.Forward[i] != null && current.Forward[i].Value.CompareTo(value) < 0) current = current.Forward[i];
        }
        current = current.Forward[0];
        return current != null && current.Value.Equals(value);
    }
}
