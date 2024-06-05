namespace Eli.Collections;

public class DynamicArray<T> where T : notnull
{
    private T[] Array { get; set; }
    private int Capacity { get; set; }
    private int Count { get; set; }

    public DynamicArray(int initialCapacity = 4)
    {
        Capacity = initialCapacity;
        Array = new T[Capacity];
        Count = 0;
    }

    public void Add(T item, int index)
    {
        while(Capacity < index) Resize();
        Array[index] = item;
        Count = Capacity - 1;
    }

    public void Add(T item)
    {
        if(Count == Capacity) Resize();
        Array[Count] = item;
        Count++;
    }

    public T this[int index] => (index < 0 || index >= Count) ? default! : Array[index];

    private void Resize()
    {
        Capacity *= 2;
        var newArray = new T[Capacity];
        System.Array.Copy(Array, newArray, Count);
        Array = newArray;
    }
}
