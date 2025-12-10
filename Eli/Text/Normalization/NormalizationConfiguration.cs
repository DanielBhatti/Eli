using System.Globalization;
using System.Text;

namespace Eli.Text.Normalization;

public class NormalizationConfiguration
{
    public static NormalizationConfiguration Default { get; } = new NormalizationConfiguration();

    public bool NormalizeUnicode { get; init; } = true;
    public NormalizationForm UnicodeForm { get; init; } = NormalizationForm.FormKC;

    public bool RemoveDiacritics { get; init; } = true;
    public bool IgnoreCase { get; init; } = true;
    public CultureInfo Culture { get; init; } = CultureInfo.InvariantCulture;

    public bool TrimWhitespace { get; init; } = true;
    public bool CollapseInternalWhitespace { get; init; } = true;

    public bool IgnorePunctuation { get; init; } = false;
}
