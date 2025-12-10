using System.Globalization;
using System.Text;

namespace Eli.Text.Normalization;

public sealed class Normalizer
{
    public NormalizationConfiguration Configuration { get; }

    public Normalizer()
        : this(NormalizationConfiguration.Default)
    { }

    public Normalizer(NormalizationConfiguration configuration) => Configuration = configuration;

    public string Normalize(string text)
    {
        if(string.IsNullOrEmpty(text)) return string.Empty;

        var s = text;
        if(Configuration.NormalizeUnicode) s = s.Normalize(Configuration.UnicodeForm);
        if(Configuration.RemoveDiacritics) s = RemoveDiacritics(s);
        if(Configuration.IgnorePunctuation) s = RemovePunctuation(s);
        if(Configuration.CollapseInternalWhitespace) s = CollapseWhitespace(s);
        if(Configuration.TrimWhitespace) s = s.Trim();
        if(Configuration.IgnoreCase) s = s.ToUpper(Configuration.Culture);
        return s;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach(var c in normalized)
        {
            var cat = CharUnicodeInfo.GetUnicodeCategory(c);
            if(cat is not UnicodeCategory.NonSpacingMark and
                not UnicodeCategory.SpacingCombiningMark and
                not UnicodeCategory.EnclosingMark)
            {
                _ = sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string RemovePunctuation(string text)
    {
        var sb = new StringBuilder(text.Length);
        foreach(var c in text)
        {
            if(!char.IsPunctuation(c)) _ = sb.Append(c);
        }
        return sb.ToString();
    }

    private static string CollapseWhitespace(string text)
    {
        var sb = new StringBuilder(text.Length);
        var inWhitespace = false;

        foreach(var c in text)
        {
            if(char.IsWhiteSpace(c))
            {
                if(!inWhitespace)
                {
                    _ = sb.Append(' ');
                    inWhitespace = true;
                }
            }
            else
            {
                _ = sb.Append(c);
                inWhitespace = false;
            }
        }

        return sb.ToString();
    }
}
