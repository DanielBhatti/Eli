namespace Eli.Data.DataSourceComparison.DataSources;

public interface DataSource
{
    string Name { get; }
    DataSourceType DataSourceType { get; }
    DataFormat DataFormat { get; }
    IReadOnlySet<string> HeaderKeys { get; }

    IReadOnlyCollection<IReadOnlyDictionary<string, string>> FieldToValueCollection { get; }
    IReadOnlyDictionary<string, IReadOnlyCollection<string>> FieldToCollection =>
        FieldToValueCollection
            .SelectMany(dict => dict)
            .GroupBy(kv => kv.Key)
            .ToDictionary(
                g => g.Key,
                g => (IReadOnlyCollection<string>)[.. g.Select(kv => kv.Value)]
            );
}
