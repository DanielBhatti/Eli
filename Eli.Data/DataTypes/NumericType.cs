using System.Globalization;

namespace Eli.Data.DataTypes;

public record class NumericType : DataType<decimal>
{
    public override DataTypeName Name => DataTypeName.Numeric;

    public required int Precision { get; init; }
    public required int Scale { get; init; }

    public bool PositiveDefinite { get; init; } = false;
    public bool NegativeDefinite { get; init; } = false;

    public decimal MaxValue { get; init; } = decimal.MaxValue;
    public decimal MinValue { get; init; } = decimal.MinValue;

    public override bool TryConvert(string value, out decimal result)
    {
        result = default;
        if(string.IsNullOrWhiteSpace(value)) return IsNullable;
        if(!decimal.TryParse(value, NumberStyles.Number | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var parsed)) return false;
        if(parsed < MinValue || parsed > MaxValue) return false;
        if(PositiveDefinite && parsed <= 0) return false;
        if(NegativeDefinite && parsed >= 0) return false;
        if(Precision > 0 || Scale > 0)
        {
            var abs = Math.Abs(parsed);
            var digits = abs == 0m ? 1 : (int)Math.Floor(Math.Log10((double)abs)) + 1;
            var scaleDigits = 0;
            var fractional = abs - Math.Truncate(abs);
            if(fractional != 0m)
            {
                var s = fractional.ToString(CultureInfo.InvariantCulture);
                var idx = s.IndexOf('.');
                if(idx >= 0) scaleDigits = s.Length - idx - 1;
            }
            if(Precision > 0 && digits + scaleDigits > Precision) return false;
            if(Scale > 0 && scaleDigits > Scale) return false;
        }
        result = parsed;
        return true;
    }

    public override RelationType Compare(decimal left, decimal right)
    {
        if(left < right) return RelationType.LessThan;
        if(right > left) return RelationType.GreaterThan;
        return RelationType.EqualTo;
    }
}
