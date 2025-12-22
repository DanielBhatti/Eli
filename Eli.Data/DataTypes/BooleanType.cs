
namespace Eli.Data.DataTypes;

public record class BooleanType : DataType<bool>
{
    public override DataTypeName Name => DataTypeName.Boolean;

    public override bool TryConvert(string value, out bool result)
    {
        result = default;
        if(new List<string>() { "1", "t", "true", "y", "yes" }.Contains(value, StringComparer.OrdinalIgnoreCase))
        {
            result = true;
            return true;
        }
        else if(new List<string>() { "0", "f", "false", "n", "no" }.Contains(value, StringComparer.OrdinalIgnoreCase))
        {
            result = false;
            return true;
        }
        return false;
    }

    public override RelationType Compare(bool left, bool right)
    {
        if(left == right) return RelationType.EqualTo;
        else return RelationType.NotEqualTo;
    }
}
