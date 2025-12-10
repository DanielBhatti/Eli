using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Frozen;
using System.Globalization;

namespace Eli.Data.DataSourceComparison.DataSources;

public class CsvFileDataSource : DataSource
{
    public string Name => "Csv";
    public DataSourceType DataSourceType => DataSourceType.File;
    public DataFormat DataFormat => DataFormat.Csv;
    public static readonly CsvConfiguration DefaultConfiguration = new(CultureInfo.InvariantCulture)
    {
        DetectDelimiter = true,
    };

    public required IReadOnlySet<string> HeaderKeys { get; init; }

    private readonly List<Dictionary<string, string>> _fieldToValueCollection = new();
    public IReadOnlyCollection<IReadOnlyDictionary<string, string>> FieldToValueCollection => _fieldToValueCollection;

    public CsvFileDataSource(string filePath) : this(filePath, DefaultConfiguration)
    { }

    public CsvFileDataSource(string filePath, CsvConfiguration csvConfiguration)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, csvConfiguration);

        while(csv.Read())
        {
            var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            HeaderKeys = csv.HeaderRecord?.ToFrozenSet() ?? throw new ArgumentNullException($"Could not find a header for the file {filePath}.");
            foreach(var header in csv.HeaderRecord.Where(h => h != null))
            {
                var field = csv.GetField(header);
                if(!row.TryAdd(header, field ?? "")) throw new Exception($"A duplicate header ({header}) when reading from file {filePath}.  Please note that headers are read by ignoring case.");
            }
            _fieldToValueCollection.Add(row);
        }
    }
}
