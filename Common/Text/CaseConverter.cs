using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Text;

public static partial class CaseConverter
{
    public static string ConvertToCase(this string input, CaseType caseType) =>
        caseType switch
        {
            CaseType.PascalCase => ToPascalCase(input),
            CaseType.SnakeCase => ToSnakeCase(input),
            CaseType.CamelCase => ToCamelCase(input),
            CaseType.SpacedPascalCase => ToSpacedPascalCase(input),
            CaseType.SpacedLowerCase => ToSpacedLowerCase(input),
            CaseType.SpacedCase => ToSpacedCase(input),
            _ => throw new ArgumentException($"Converstion to {nameof(CaseType)} {caseType} not currently supported."),
        };

    public static CaseType DetermineCase(this string input)
    {
        if(input.IsPascalCase()) return CaseType.PascalCase;
        else if(input.IsSnakeCase()) return CaseType.SnakeCase;
        else if(input.IsCamelCase()) return CaseType.CamelCase;
        else if(input.IsSpacedPascalCase()) return CaseType.SpacedPascalCase;
        else if(input.IsSpacedLowerCase()) return CaseType.SpacedLowerCase;
        else if(input.IsSpacedCase()) return CaseType.SpacedCase;
        else return CaseType.Unknown;
    }

    public static CaseType GetCaseType(this string input)
    {
        if(IsPascalCase(input)) return CaseType.PascalCase;
        else if(IsSnakeCase(input)) return CaseType.SnakeCase;
        else if(IsCamelCase(input)) return CaseType.CamelCase;
        else if(IsSpacedPascalCase(input)) return CaseType.SpacedPascalCase;
        else if(IsSpacedLowerCase(input)) return CaseType.SpacedLowerCase;
        else if(IsSpacedCase(input)) return CaseType.SpacedCase;
        else return CaseType.Unknown;
    }

    public static string ToPascalCase(this string input) => string.Concat(input.GetWordParts().Select(w => char.ToUpper(w[0]) + w[1..]));

    public static string ToSnakeCase(this string input) => string.Join("_", input.GetWordParts().Select(w => w.ToLower()));

    public static string ToCamelCase(this string input) => string.Concat(input.GetWordParts().Select((w, i) => i == 0 ? w.ToLower() : char.ToUpper(w[0]) + w[1..]));

    public static string ToSpacedPascalCase(this string input) => string.Join(" ", input.GetWordParts().Select(w => char.ToUpper(w[0]) + w[1..]));

    public static string ToSpacedLowerCase(this string input) => string.Join(" ", input.GetWordParts()).ToLower();

    public static string ToSpacedCase(this string input) => string.Join(" ", input.GetWordParts());

    public static bool IsPascalCase(this string input) => PascalCaseRegex().IsMatch(input);

    public static bool IsSnakeCase(this string input) => SnakeCaseRegex().IsMatch(input);

    public static bool IsCamelCase(this string input) => CamelCaseRegex().IsMatch(input);

    public static bool IsSpacedPascalCase(this string input) => SpacedPascalCaseRegex().IsMatch(input);

    public static bool IsSpacedLowerCase(this string input) => SpacedLowerCaseRegex().IsMatch(input);

    public static bool IsSpacedCase(this string input) => SpacedCaseRegex().IsMatch(input);

    public static string[] GetWordParts(this string input) => input.GetCaseType() switch
        {
            CaseType.PascalCase or CaseType.CamelCase => SplitPascalOrCamelCaseRegex().Split(input),
            CaseType.SnakeCase => input.Split('_'),
            CaseType.SpacedPascalCase => input.Split(' '),
            CaseType.SpacedLowerCase => input.Split(' '),
            CaseType.SpacedCase => input.Split(' '),
            _ => new string[] { input },
        };

    [GeneratedRegex("^([A-Z][a-z0-9]+)*$")]
    private static partial Regex PascalCaseRegex();

    [GeneratedRegex("^[a-z0-9_]+$")]
    private static partial Regex SnakeCaseRegex();

    [GeneratedRegex("^[a-z]+(?:[A-Z][a-z0-9]+)*$")]
    private static partial Regex CamelCaseRegex();

    [GeneratedRegex(@"^[A-Z][a-z0-9]*(?:\s[A-Z][a-z0-9]*)*$")]
    private static partial Regex SpacedPascalCaseRegex();

    [GeneratedRegex(@"^[a-z0-9]*(?:\s[a-z0-9]*)*$")]
    private static partial Regex SpacedLowerCaseRegex();

    [GeneratedRegex(@"^[A-Za-z0-9]*(?:\s[A-Za-z0-9]*)*$")]
    private static partial Regex SpacedCaseRegex();

    [GeneratedRegex("(?<!^)(?=[A-Z])")]
    private static partial Regex SplitPascalOrCamelCaseRegex();
}
