using Eli.Data.DataSourceComparison;
using Eli.Data.DataSourceComparison.DataSources;

namespace Eli.Data.Test.DataSourceComparison;

[TestFixture]
public class ComparatorTests
{
    private sealed class TestDataSource : DataSource
    {
        public string Name { get; init; } = string.Empty;

        public DataSourceType DataSourceType { get; init; }
        public DataFormat DataFormat { get; init; }

        public IReadOnlySet<string> HeaderKeys { get; init; }
            = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<IReadOnlyDictionary<string, string>> FieldToValueCollection { get; init; }
            = new List<IReadOnlyDictionary<string, string>>();
    }

    [Test]
    public void Compare_Throws_WhenLeftHasRowMissingPrimaryKeyField()
    {
        var left = new TestDataSource
        {
            Name = "Left",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string> { { "Id", "1" }, { "Name", "Alice" } },
                new Dictionary<string, string> { { "Name", "Bob" } }
            }
        };

        var right = new TestDataSource
        {
            Name = "Right",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>()
        };

        var comparator = new Comparator();

        Assert.Throws<Exception>(() => comparator.Compare(left, right, "Id"));
    }

    [Test]
    public void Compare_Throws_WhenRightHasDuplicatePrimaryKeyCombination()
    {
        var left = new TestDataSource
        {
            Name = "Left",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>()
        };

        var right = new TestDataSource
        {
            Name = "Right",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string> { { "Id", "1" }, { "Name", "Alice" } },
                new Dictionary<string, string> { { "Id", "1" }, { "Name", "Bob" } } // Duplicate PK
            }
        };

        var comparator = new Comparator();

        Assert.Throws<Exception>(() => comparator.Compare(left, right, "Id"));
    }

    [Test]
    public void Compare_ProducesMissingRightRow_WhenPkOnlyInLeft()
    {
        var left = new TestDataSource
        {
            Name = "Left",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string> { { "Id", "1" }, { "Name", "Alice" } }
            }
        };

        var right = new TestDataSource
        {
            Name = "Right",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>()
        };

        var comparator = new Comparator();

        var results = comparator.Compare(left, right, "Id");

        Assert.That(results, Has.Count.EqualTo(1));
        var result = results[0];

        Assert.That(result.ComparisonResultType, Is.EqualTo(ComparisonResultType.MissingRightRow));
        Assert.That(result.PredictedDataType, Is.Null);
        Assert.That(result.LeftValue, Is.Not.Null);
        Assert.That(result.RightValue, Is.Null);
        Assert.That(result.PrimaryKey, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void Compare_ProducesMissingLeftRow_WhenPkOnlyInRight()
    {
        var left = new TestDataSource
        {
            Name = "Left",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>()
        };

        var right = new TestDataSource
        {
            Name = "Right",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string> { { "Id", "1" }, { "Name", "Alice" } }
            }
        };

        var comparator = new Comparator();

        var results = comparator.Compare(left, right, "Id");

        Assert.That(results, Has.Count.EqualTo(1));
        var result = results[0];

        Assert.That(result.ComparisonResultType, Is.EqualTo(ComparisonResultType.MissingLeftRow));
        Assert.That(result.PredictedDataType, Is.Null);
        Assert.That(result.LeftValue, Is.Null);
        Assert.That(result.RightValue, Is.Not.Null);
        Assert.That(result.PrimaryKey, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void Compare_ProducesRightValueHeaderFieldMissing_WhenFieldMissingOnRight()
    {
        // Same PK on both sides, but right side is missing one field ("Name").
        var left = new TestDataSource
        {
            Name = "Left",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "Id", "1" },
                    { "Name", "Alice" }
                }
            }
        };

        var right = new TestDataSource
        {
            Name = "Right",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "Id", "1" }
                }
            }
        };

        var comparator = new Comparator();

        var results = comparator.Compare(left, right, "Id");

        // We expect exactly one result describing the missing "Name" field on the right.
        Assert.That(results, Has.Count.EqualTo(1));
        var result = results[0];

        Assert.That(result.ComparisonResultType, Is.EqualTo(ComparisonResultType.RightValueHeaderFieldMissing));
        Assert.That(result.PredictedDataType, Is.Null);
        Assert.That(result.LeftValue, Is.EqualTo("Alice"));
        Assert.That(result.RightValue, Is.Null);
        Assert.That(result.PrimaryKey, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void Compare_ProducesLeftValueHeaderFieldMissing_WhenFieldMissingOnLeft()
    {
        var left = new TestDataSource
        {
            Name = "Left",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "Id", "1" }
                }
            }
        };

        var right = new TestDataSource
        {
            Name = "Right",
            FieldToValueCollection = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "Id", "1" },
                    { "Name", "Alice" }
                }
            }
        };

        var comparator = new Comparator();

        var results = comparator.Compare(left, right, "Id");

        // We expect exactly one result describing the missing "Name" field on the left.
        Assert.That(results, Has.Count.EqualTo(1));
        var result = results[0];

        Assert.That(result.ComparisonResultType, Is.EqualTo(ComparisonResultType.LeftValueHeaderFieldMissing));
        Assert.That(result.PredictedDataType, Is.Null);
        Assert.That(result.LeftValue, Is.Null);
        Assert.That(result.RightValue, Is.EqualTo("Alice"));
        Assert.That(result.PrimaryKey, Is.Not.Null.And.Not.Empty);
    }
}
