
using System.Globalization;

namespace Eli.Data.DataTypes;

public record class IntegerType : DataType<int>
{
    public override DataTypeName Name => DataTypeName.Integer;

    public bool PositiveDefinite { get; init; } = false;
    public bool NegativeDefinite { get; init; } = false;
    public int MaxValue { get; init; } = int.MaxValue;
    public int MinValue { get; init; } = int.MinValue;

    public override bool TryConvert(string value, out int result)
    {
        result = default;
        if(string.IsNullOrWhiteSpace(value)) return IsNullable;
        if(!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)) return false;
        if(parsed < MinValue || parsed > MaxValue) return false;
        if(PositiveDefinite && parsed <= 0) return false;
        if(NegativeDefinite && parsed >= 0) return false;
        result = parsed;
        return true;
    }

    public override RelationType Compare(int left, int right)
    {
        if(left < right) return RelationType.LessThan;
        if (right > left) return RelationType.GreaterThan;
        return RelationType.EqualTo;
    }
}
