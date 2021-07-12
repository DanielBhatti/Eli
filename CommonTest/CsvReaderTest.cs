using Common.IO;
using NUnit.Framework;

namespace CommonTest
{
    [TestFixture]
    public class CsvReaderTest
    {
        private CsvReader _csvReader;

        [SetUp]
        public void SetUp()
        {
            _csvReader = new CsvReader();
        }

        [Test]
        public void ReadCsv()
        {
            string filePath = "";
            _csvReader.FileToDataTable(filePath);
        }
    }
}
