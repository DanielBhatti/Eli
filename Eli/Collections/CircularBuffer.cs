using System;
using System.Collections;
using System.Collections.Generic;

namespace Eli.Collections;

public class CircularBuffer<T> : IEnumerable<T>, ICollection, IReadOnlyCollection<T>
{
    public T? this[int index] => Peek(index);

    private readonly T[] Buffer;
    private int Head { get; set; } = 0;
    private int Tail { get; set; } = 0;
    
    public int Count { get; set; } = 0;
    public int Capacity { get; }

    public CircularBuffer(int capacity)
    {
        if(capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        Capacity = capacity;
        Buffer = new T[capacity];
    }

    public void Enqueue(T item)
    {
        if(Count == Capacity) Head = (Head + 1) % Capacity;
        else Count++;
        Buffer[Tail] = item;
        Tail = (Tail + 1) % Capacity;
    }

    public T? Dequeue()
    {
        if(Count == 0) return default;
        var item = Buffer[Head];
        Head = (Head + 1) % Capacity;
        Count--;
        return item;
    }

    public T? Peek(int index)
    {
        if(index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        return Buffer[(Head + index) % Capacity];
    }

    public IEnumerator<T> GetEnumerator()
    {
        for(var i = 0; i < Count; i++) yield return Buffer[(Head + i) % Capacity];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(T item) => Enqueue(item);

    public void Clear()
    {
        Head = 0;
        Tail = 0;
        Count = 0;
    }

    public bool Contains(T item)
    {
        for(var i = 0; i < Count; i++) if(EqualityComparer<T>.Default.Equals(Buffer[(Head + i) % Capacity], item)) return true;
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        for(var i = 0; i < Count; i++) array[arrayIndex + i] = Buffer[(Head + i) % Capacity];
    }

    public void CopyTo(Array array, int index)
    {
        for(var i = 0; i < Count; i++) array.SetValue(Buffer[(Head + i) % Capacity], index + i);
    }

    public bool IsSynchronized => false;
    public object SyncRoot => this;
}
