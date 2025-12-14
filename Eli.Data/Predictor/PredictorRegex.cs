using System.Text.RegularExpressions;

namespace Eli.Data.Predictor;

public static partial class PredictorRegex
{
    [GeneratedRegex(@"^.$")]
    public static partial Regex CharRegex();

    [GeneratedRegex(@"^(?i:true|false|t|f|0|1|yes|no|y|n)$")]
    public static partial Regex BooleanRegex();

    [GeneratedRegex(@"^[+-]?\d+$")]
    public static partial Regex IntegerRegex();

    [GeneratedRegex(@"^\$?[+-]?(\d+(\.\d+)?|\.\d+)$")]
    public static partial Regex NumericRegex();

    [GeneratedRegex(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$")]
    public static partial Regex DateRegex();

    [GeneratedRegex(@"^([01]\d|2[0-3]):[0-5]\d(:[0-5]\d(\.\d+)?)?$")]
    public static partial Regex TimeRegex();

    [GeneratedRegex(@"^\d{4}-\d{2}-\d{2}[T ]([01]\d|2[0-3]):[0-5]\d(:[0-5]\d(\.\d+)?)?$")]
    public static partial Regex DateTimeRegex();

    [GeneratedRegex(@"^\d{4}-\d{2}-\d{2}[T ]([01]\d|2[0-3]):[0-5]\d(:[0-5]\d(\.\d+)?)?(Z|[+-]([01]\d|2[0-3]):[0-5]\d)$")]
    public static partial Regex DateTimeOffsetRegex();

    [GeneratedRegex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$")]
    public static partial Regex GuidRegex();
}
