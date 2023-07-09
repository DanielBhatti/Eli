using Common.IO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;

namespace Common.Test;

[TestFixture]
public class CsvReaderTest
{
    [SetUp]
    public void SetUp() { }

    [Test]
    public void ReadCsv()
    {
        // change filepath logic later
        var filePath = System.IO.Path.Combine(@"C:\Users\bhatt\repos\Common\Common.Test\Resources", "SampleCsv.csv");
        var types = new Type[7];
        for(var i = 0; i < 7; i++) types[i] = typeof(int);

        var dataTable = CsvReader.FileToDataTable(filePath, true, types: types);

        Assert.AreEqual("0", dataTable.Rows[0]["a"]);
        Assert.AreEqual("1", dataTable.Rows[0]["b"]);
        Assert.AreEqual("2", dataTable.Rows[0]["c"]);
        Assert.AreEqual("0", dataTable.Rows[1]["a"]);
        Assert.AreEqual("2", dataTable.Rows[1]["b"]);
        Assert.AreEqual("4", dataTable.Rows[1]["c"]);
        Assert.AreEqual("1", dataTable.Rows[2]["a"]);
        Assert.AreEqual("1", dataTable.Rows[2]["b"]);
        Assert.AreEqual("1", dataTable.Rows[2]["c"]);
    }
}
