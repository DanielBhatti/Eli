using Common.IO;
using NUnit.Framework;
using System;
using System.Data;

namespace CommonTest
{
    [TestFixture]
    public class CsvReaderTest
    {
        private CsvReader _csvReader;
        private StartUpManager _sum;

        [SetUp]
        public void SetUp()
        {
            _csvReader = new CsvReader();
            _sum = new StartUpManager();
        }

        [Test]
        public void ReadCsv()
        {
            string filePath = System.IO.Path.Combine(StartUpManager.ResourcesDirectory, "SampleCsv.csv");
            Type[] types = new Type[7];
            for (int i = 0; i < 7; i++) types[i] = typeof(int);

            DataTable dataTable = _csvReader.FileToDataTable(filePath, true, types: types);

            foreach(DataColumn dataColumn in dataTable.Columns)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    object val = dataTable.Rows[i][dataColumn];
                    if (i == 0) Assert.AreEqual((int)Math.Pow(i, 2), val);
                    if (i == 1) Assert.AreEqual(2 * i, val);
                    if (i == 2) Assert.AreEqual(1, val);
                }
            }
        }
    }
}
