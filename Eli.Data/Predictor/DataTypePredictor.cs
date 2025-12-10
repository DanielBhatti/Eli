using Eli.Data.DataTypes;

namespace Eli.Data.Predictor;

public sealed class DataTypePredictor
{
    public DataType Predict(IEnumerable<string> values)
    {
        return new StringType() { Length = int.MaxValue, IsNullable = false };
    }

    public DataType Predict(string value) => Predict(new[] { value });
}
