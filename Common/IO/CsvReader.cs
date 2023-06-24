using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.IO;

public class CsvReader
{
    public DataTable FileToDataTable(string filePath, bool hasHeader = false, string delimiter = ",", Type[] types = null)
    {
        var dt = new DataTable();
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = hasHeader,
            Delimiter = delimiter,
        };
        using(var reader = new StreamReader(filePath, Encoding.Default))
        using(var csv = new CsvHelper.CsvReader(reader, config))
        {
            var records = csv.GetRecords<dynamic>().ToList();
            if(records.Count == 0)
            {
                return dt;
            }

            if(records[0] is not IDictionary<string, object> firstRecord)
            {
                throw new Exception("Failed to read the CSV file.");
            }

            foreach(var key in firstRecord.Keys) _ = dt.Columns.Add(key);

            if(types != null && types.Length == dt.Columns.Count) for(var i = 0; i < types.Length; i++) dt.Columns[i].DataType = types[i];

            foreach(var record in records)
            {
                var row = dt.NewRow();
                var dict = record as IDictionary<string, object>;
                foreach(DataColumn column in dt.Columns) row[column.ColumnName] = dict[column.ColumnName];
                dt.Rows.Add(row);
            }
        }
        return dt;
    }
}
