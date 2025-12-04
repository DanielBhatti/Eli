namespace Eli.Data.Comparison.DataSources;

public interface DataSource
{
    DataSourceType DataSourceType { get; }
    DataFormat DataFormat { get; }

    ICollection<IDictionary<string, object>> FieldMap { get; }
}
