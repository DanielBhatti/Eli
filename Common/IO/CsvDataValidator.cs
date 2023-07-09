using System;
using System.Data;
using System.IO;
using System.Linq;

namespace Common.IO;

public class CsvDataValidator
{
    private readonly CsvReader _csvReader = new();
    private readonly DataTable _dataTable;

    public string[] Headers { get; }
    public Type[] HeaderTypes { get; }
    public int NumberOfRows { get; }
    public int NumberOfColumns { get; }

    public CsvDataValidator(string filePath, bool hasHeader = false,
        string delimiter = ",", Type[] types = null)
    {
        if(types is null)
        {
            using StreamReader reader = new(filePath);
            if(hasHeader)
            {
                _ = reader.ReadLine();
            }

            while(!reader.EndOfStream)
            {

            }
        }
        _dataTable = CsvReader.FileToDataTable(filePath, hasHeader, delimiter, types);
        Headers = _dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
    }
}
