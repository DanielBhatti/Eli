using System;
using System.Data;
using System.IO;

namespace Common.IO;

public class CsvReader
{
    public DataTable FileToDataTable(string filePath, bool hasHeader = false,
        string delimiter = ",", Type[] types = null)
    {
        DataTable dataTable = new();
        using(StreamReader reader = new(filePath))
        {
            var firstLine = reader.ReadLine();
            var numFields = firstLine.Split(delimiter).Length;
            ToStart(reader);

            if(types is null)
            {
                types = new Type[numFields];
                for(var i = 0; i < types.Length; i++)
                {
                    types[i] = typeof(string);
                }
            }
            else if(types.Length != numFields)
            {
                throw new ArgumentException("Number of types provided must equal number of fields in the file.");
            }

            if(hasHeader)
            {
                var line = reader.ReadLine();
                var headerFields = line.Split(delimiter);
                foreach(var field in headerFields)
                {
                    _ = dataTable.Columns.Add(field);
                }
            }

            while(!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                object[] fields = line.Split(delimiter);
                var convertedFields = new object[fields.Length];
                for(var i = 0; i < fields.Length; i++)
                {
                    convertedFields[i] = Convert.ChangeType(fields[i], types[i]);
                }
                _ = dataTable.Rows.Add(convertedFields);
            }
        }
        return dataTable;
    }

    private void ToStart(StreamReader reader)
    {
        reader.DiscardBufferedData();
        _ = reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
    }
}
