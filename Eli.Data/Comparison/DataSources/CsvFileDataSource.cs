using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Eli.Data.Comparison.DataSources;

public class CsvFileDataSource : DataSource
{
    private readonly List<IDictionary<string, object>> _records;

    public DataSourceType DataSourceType => DataSourceType.File;
    public DataFormat DataFormat => DataFormat.Csv;

    public ICollection<IDictionary<string, object>> FieldMap => _records;

    public CsvFileDataSource(string filePath) : this(filePath, new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        DetectDelimiter = true,
        BadDataFound = null,
        MissingFieldFound = null,
        HeaderValidated = null
    })
    { }

    public CsvFileDataSource(string filePath, CsvConfiguration csvConfiguration)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, csvConfiguration);

        _records = new List<IDictionary<string, object>>();

        while(csv.Read())
        {
            var row = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if(csv.HeaderRecord != null)
            {
                foreach(var header in csv.HeaderRecord)
                {
                    var raw = csv.GetField(header);
                    row[header] = ParseValue(raw);
                }
            }

            _records.Add(row);
        }
    }

    private static object ParseValue(string? raw)
    {
        if(raw is null) return string.Empty;

        if(int.TryParse(raw, out var i)) return i;
        if(long.TryParse(raw, out var l)) return l;
        if(double.TryParse(raw, out var d)) return d;
        if(decimal.TryParse(raw, out var m)) return m;
        if(bool.TryParse(raw, out var b)) return b;
        if(DateTime.TryParse(raw, out var dt)) return dt;

        return raw;
    }
}
