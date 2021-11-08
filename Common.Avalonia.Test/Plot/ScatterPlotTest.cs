using Avalonia.Controls;
using NUnit.Framework;
using System;

namespace Common.Avalonia.Test.Plot
{
    [Apartment(System.Threading.ApartmentState.STA)]
    public class ScatterPlotTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PlotDoubles()
        {
            Window window = new Window();
            
            Assert.Pass();
        }
    }
}