using System;
using System.Data;
using System.IO;

namespace Common.IO
{
    public class CsvReader
    {
        public DataTable FileToDataTable(string filePath, bool hasHeader = false,
            string delimiter = ",", Type[] types = null)
        {
            DataTable dataTable = new DataTable();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string firstLine = reader.ReadLine();
                int numFields = firstLine.Split(delimiter).Length;
                ToStart(reader);

                if (types is null)
                {
                    types = new Type[numFields];
                    for (int i = 0; i < types.Length; i++) types[i] = typeof(string);
                }
                else if (types.Length != numFields)
                    throw new ArgumentException("Number of types provided must equal number of fields in the file.");

                if (hasHeader)
                {
                    string line = reader.ReadLine();
                    string[] headerFields = line.Split(delimiter);
                    foreach(string field in headerFields)
                    {
                        dataTable.Columns.Add(field);
                    }
                }

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    object[] fields = line.Split(delimiter);
                    object[] convertedFields = new object[fields.Length];
                    for (int i = 0; i < fields.Length; i++)
                    {
                        convertedFields[i] = Convert.ChangeType(fields[i], types[i]);
                    }
                    dataTable.Rows.Add(convertedFields);
                }
            }
            return dataTable;
        }

        private void ToStart(StreamReader reader)
        {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
        }
    }
}
