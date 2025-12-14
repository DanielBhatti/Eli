using Eli.Data.DataTypes;
using Eli.Data.Predictor;

namespace Eli.Data.Test.Predictor;

[TestFixture]
public sealed class DataTypePredictorTests
{
    private static T SingleOfType<T>(IEnumerable<DataType> predictions) where T : DataType =>
        predictions.OfType<T>().Single();

    [Test]
    public void Predict_ReturnsUnknown_WhenNoPredictions()
    {
        var predictor = new DataTypePredictor(new PredictorOptions
        {
            PredictionPrecedence = Array.Empty<DataTypeName>()
        });

        var result = predictor.Predict(Array.Empty<string>());

        Assert.That(result, Is.TypeOf<UnknownDataType>());
    }

    [Test]
    public void Predict_UsesPredictionPrecedence_WhenMultipleMatch()
    {
        var options = new PredictorOptions
        {
            PredictionPrecedence = new[]
            {
                DataTypeName.Integer,
                DataTypeName.Character,
                DataTypeName.Numeric,
                DataTypeName.String,
                DataTypeName.Unknown,
            }
        };

        var predictor = new DataTypePredictor(options);

        var result = predictor.Predict(new[] { "2", "3", "9" });

        Assert.That(result, Is.TypeOf<IntegerType>());
        Assert.That(result.IsNullable, Is.False);
    }

    [Test]
    public void PredictAll_ContainsStringPrediction_ForNonEmptyText()
    {
        var predictor = new DataTypePredictor();

        var predictions = predictor.PredictAll(new[] { "abc", "def" });

        Assert.That(predictions.OfType<StringType>().Count(), Is.EqualTo(1));
        var str = predictions.OfType<StringType>().Single();
        Assert.That(str.IsNullable, Is.False);
        Assert.That(str.MaxLength, Is.EqualTo(3));
        Assert.That(str.IsFixedLength, Is.True);
    }

    [Test]
    public void PredictAll_SetsIsNullable_WhenAnyValueNormalizesToEmpty()
    {
        var predictor = new DataTypePredictor();
        var predictions = predictor.PredictAll(new[] { "", "abc" });

        var str = SingleOfType<StringType>(predictions);
        Assert.That(str.IsNullable, Is.True);
    }

    [Test]
    public void PredictAll_SetsIsFixedLength_True_WhenAllNonEmptyValuesSameLength()
    {
        var predictor = new DataTypePredictor();
        var predictions = predictor.PredictAll(new[] { "aa", "bb", "cc" });

        var str = SingleOfType<StringType>(predictions);
        Assert.That(str.MaxLength, Is.EqualTo(2));
        Assert.That(str.IsFixedLength, Is.True);
        Assert.That(str.IsNullable, Is.False);
    }

    [Test]
    public void PredictAll_SetsIsFixedLength_False_WhenLengthsVary()
    {
        var predictor = new DataTypePredictor();
        var predictions = predictor.PredictAll(new[] { "a", "bb", "ccc" });

        var str = SingleOfType<StringType>(predictions);
        Assert.That(str.MaxLength, Is.EqualTo(3));
        Assert.That(str.IsFixedLength, Is.False);
    }

    [Test]
    public void PredictAll_IncludesCharacter_WhenAllValuesAreLengthOne_AndNonBoolean()
    {
        var predictor = new DataTypePredictor();
        var predictions = predictor.PredictAll(new[] { "2", "A", "z" });

        Assert.That(predictions.OfType<CharType>().Count(), Is.EqualTo(1));
        Assert.That(predictions.OfType<CharType>().Single().IsNullable, Is.False);
    }

    [Test]
    public void PredictAll_IncludesInteger_WhenAllValuesMatchIntegerRegex()
    {
        var predictor = new DataTypePredictor();
        var predictions = predictor.PredictAll(new[] { "-12", "0", "345" });

        Assert.That(predictions.OfType<IntegerType>().Count(), Is.EqualTo(1));
    }
}
