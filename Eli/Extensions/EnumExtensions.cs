using Eli.Text;
using System;

namespace Eli.Extensions;

internal static class EnumExtensions
{
    public static string ToSpacedString(this Enum enumValue) => enumValue.ToString().ToSpacedPascalCase();

    public static T FromString<T>(this string value, bool ignoreCase = true) where T : Enum
    {
        var enumType = typeof(T);
        foreach(var name in Enum.GetNames(enumType)) if(string.Equals(name, value.Replace(" ", ""), ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) return (T)Enum.Parse(enumType, name, ignoreCase);
        throw new ArgumentException($"No enum value found for string '{value}' in {enumType.Name}");
    }
}
