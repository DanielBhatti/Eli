using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.IO
{
    public class CsvDataValidator
    {
        private CsvReader _csvReader = new CsvReader();
        private DataTable _dataTable;

        public string[] Headers { get; }
        public Type[] HeaderTypes { get; }
        public int NumberOfRows { get; }
        public int NumberOfColumns { get; }

        public CsvDataValidator(string filePath, bool hasHeader = false,
            string delimiter = ",", Type[] types = null)
        {
            if (types is null)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    if (hasHeader) reader.ReadLine();
                    while (!reader.EndOfStream)
                    {

                    }
                }
            }
            _dataTable = _csvReader.FileToDataTable(filePath, hasHeader, delimiter, types);
            Headers = _dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
        }
    }
}
