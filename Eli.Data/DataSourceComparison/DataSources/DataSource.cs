namespace Eli.Data.DataSourceComparison.DataSources;

public interface DataSource
{
    string Name { get; }
    DataSourceType DataSourceType { get; }
    DataFormat DataFormat { get; }
    IReadOnlySet<string> HeaderKeys { get; }

    IReadOnlyCollection<IReadOnlyDictionary<string, string>> FieldToValueCollection { get; }
}
