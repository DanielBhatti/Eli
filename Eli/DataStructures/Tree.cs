namespace Eli.DataStructures;

public interface Tree<TKey, TValue>
{
    void Insert(TValue value);

    void Delete(TKey value);

    TValue? Search(TKey key);
}
