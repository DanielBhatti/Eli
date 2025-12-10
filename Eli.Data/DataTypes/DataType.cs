namespace Eli.Data.DataTypes;

public interface DataType
{
    Type Type { get; }
    DataTypeName Name { get; }
    bool IsNullable { get; }

    bool TryConvert(string value, out object result);

    RelationType Compare(object left, object right);
}

public abstract record class DataType<T> : DataType where T : notnull
{
    public Type Type => typeof(T);
    public abstract DataTypeName Name { get; }
    public required bool IsNullable { get; init; }

    public abstract bool TryConvert(string value, out T result);
    public bool TryConvert(string value, out object result)
    {
        var success = TryConvert(value, out T typedResult);
        result = success ? typedResult : "";
        return success;
    }

    public abstract RelationType Compare(T left, T right);
    public RelationType Compare(object left, object right)
    {
        if(left is null || right is null) return RelationType.NonComparable;
        if(left is not T l || right is not T r) return RelationType.NonComparable;
        return Compare(l, r);
    }
}