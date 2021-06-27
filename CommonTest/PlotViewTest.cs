using NUnit.Framework;
using System.Collections.Generic;
using Common.Wpf;
using System;

namespace CommonTest
{
    [TestFixture]
    public class PlotViewTest
    {
        private StartUpManager csu;

        [SetUp]
        public void Setup()
        {
            csu = new StartUpManager();
        }

        //[Test, Apartment(System.Threading.ApartmentState.STA)]
        public void PlotDoubleData()
        {

            List<double> xData = new List<double>();
            List<double> yData = new List<double>();
            for(int i = 0; i < 10; i++)
            {
                xData.Add(i);
                yData.Add(Math.Pow(2, i));
            }

            string title = "Title";

            PlotControl plotControl = new PlotControl();
            plotControl.Title = title;
            plotControl.AddAxis(Position.Bottom, "Bottom");
            plotControl.AddAxis(Position.Left, "Left");
            plotControl.AddSeries(xData, yData, "TestName");

            csu.ShowControl(plotControl);

            Assert.AreEqual(true, true);
        }

        [Test, Apartment(System.Threading.ApartmentState.STA)]
        public void PlotDateTimeDoubleData()
        {

            List<DateTime> xData = new List<DateTime>();
            List<double> yData = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                xData.Add(new DateTime(2021, (i+1) % 12, (i+1) % 28));
                yData.Add(Math.Pow(2, i));
            }

            string title = "Title";

            PlotControl plotControl = new PlotControl();
            plotControl.Title = title;
            plotControl.AddAxis(Position.Bottom, "Bottom", AxisType.DateTime);
            plotControl.AddAxis(Position.Left, "Left");
            plotControl.AddSeries(xData, yData, "TestName");

            csu.ShowControl(plotControl);

            Assert.AreEqual(true, true);
        }
    }
}
