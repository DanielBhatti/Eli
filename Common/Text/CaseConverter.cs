using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Text;

public static class CaseConverter
{
    public static string ConvertToCase(string input, CaseType caseType) =>
        caseType switch
        {
            CaseType.PascalCase => ToPascalCase(input),
            CaseType.SnakeCase => ToSnakeCase(input),
            CaseType.CamelCase => ToCamelCase(input),
            CaseType.SpacedPascalCase => ToSpacedPascalCase(input),
            _ => throw new ArgumentException("Invalid case type."),
        };

    public static CaseType DetermineCase(string input)
    {
        if(IsPascalCase(input)) return CaseType.PascalCase;
        if(IsSnakeCase(input)) return CaseType.SnakeCase;
        if(IsCamelCase(input)) return CaseType.CamelCase;
        if(IsSpacedPascalCase(input)) return CaseType.SpacedPascalCase;
        return CaseType.Unknown;
    }

    public static string ToPascalCase(string input) => Regex.Replace(input, "(\\B[A-Z])", " $1").Split().Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()).Aggregate("", (current, word) => current + word);

    public static string ToSnakeCase(string input) => string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();

    public static string ToCamelCase(string input)
    {
        var words = input.Split(new[] { ' ', '_' }, StringSplitOptions.RemoveEmptyEntries);
        return words[0].ToLower() + string.Join(string.Empty, words.Skip(1).Select(w => char.ToUpper(w[0]) + w.Substring(1)));
    }

    public static string ToSpacedPascalCase(string input) =>
        Regex.Replace(input, "(\\B[A-Z])", " $1")
            .Split()
            .Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower())
            .Aggregate("", (current, word) => current + (current.Length > 0 ? " " : "") + word);

    public static bool IsPascalCase(string input) => Regex.IsMatch(input, @"^[A-Z][a-zA-Z0-9]*$");

    public static bool IsSnakeCase(string input) => Regex.IsMatch(input, @"^[a-z0-9_]+$");

    public static bool IsCamelCase(string input) => Regex.IsMatch(input, @"^[a-z]+(?:[A-Z][a-z0-9]+)*$");

    public static bool IsSpacedPascalCase(string input) => Regex.IsMatch(input, @"^(?:[A-Z][a-z0-9]*\s*)+$");
}
