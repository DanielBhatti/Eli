
namespace Eli.Data.DataTypes;

public class UnknownDataType : DataType
{
    public Type Type => typeof(object);

    public DataTypeName Name => DataTypeName.Unknown;

    public bool IsNullable => true;

    public RelationType Compare(object left, object right) => RelationType.NonComparable;
    public bool TryConvert(string value, out object result)
    {
        result = "";
        return false;
    }
}
