using NUnit.Framework;
using System.Collections.Generic;
using Common.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

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

            Assert.Pass();
        }

        //[Test, Apartment(System.Threading.ApartmentState.STA)]
        public void PlotDoubleDataThroughProperties()
        {
            List<double> xData = new List<double>();
            List<double> yData = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                xData.Add(i);
                yData.Add(Math.Pow(2, i));
            }

            string title = "Title";

            PlotControl plotControl = new PlotControl();
            plotControl.Title = title;

            Axis bottomAxis = new Axis("Bottom", Position.Bottom, AxisType.Linear);
            Axis leftAxis = new Axis("Left", Position.Left, AxisType.Linear);
            PlotData<double, double> plotData = new PlotData<double, double>(xData, yData, "Data", Color.Black, LineStyle.Solid, 2.0);
            plotControl.PlotAxes = new ObservableCollection<Axis>() { bottomAxis, leftAxis };
            plotControl.PlotData = new ObservableCollection<object>() { plotData };

            csu.ShowControl(plotControl);

            Assert.Pass();
        }

        //[Test, Apartment(System.Threading.ApartmentState.STA)]
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

            Assert.Pass();
        }

        //[Test, Apartment(System.Threading.ApartmentState.STA)]
        public void PlotDateTimeDoubleDataThroughProperties()
        {
            List<DateTime> xData = new List<DateTime>();
            List<double> yData = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                xData.Add(new DateTime(2021, 1, i + 1));
                yData.Add(Math.Pow(2, i));
            }

            string title = "Title";

            PlotControl plotControl = new PlotControl();
            plotControl.Title = title;

            Axis bottomAxis = new Axis("Bottom", Position.Bottom, AxisType.DateTime);
            Axis leftAxis = new Axis("Left", Position.Left, AxisType.Linear);
            PlotData<DateTime, double> plotData = new PlotData<DateTime, double>(xData, yData, "Data", Color.Black, LineStyle.Solid, 2.0);
            plotControl.PlotAxes = new ObservableCollection<Axis>() { bottomAxis, leftAxis };
            plotControl.PlotData = new ObservableCollection<object>() { plotData };

            csu.ShowControl(plotControl);

            Assert.Pass();
        }

        //[Test, Apartment(System.Threading.ApartmentState.STA)]
        public void PlotMultipleSetsOfData()
        {
            List<DateTime> xData1 = new List<DateTime>();
            List<double> yData1 = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                xData1.Add(new DateTime(2021, 1, i + 1));
                yData1.Add(Math.Pow(2, i));
            }

            List<DateTime> xData2 = new List<DateTime>();
            List<double> yData2 = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                xData2.Add(new DateTime(2021, 1, i + 1));
                yData2.Add(Math.Pow(3, i));
            }

            List<DateTime> xData3 = new List<DateTime>();
            List<double> yData3 = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                xData3.Add(new DateTime(2021, 1, i + 1));
                yData3.Add(Math.Pow(4, i));
            }

            string title = "Title";

            PlotControl plotControl = new PlotControl();
            plotControl.Title = title;

            Axis bottomAxis = new Axis("Bottom", Position.Bottom, AxisType.DateTime);
            Axis leftAxis = new Axis("Left", Position.Left, AxisType.Linear);
            PlotData<DateTime, double> plotData1 = new PlotData<DateTime, double>(xData1, yData1, "Data", Color.Black, LineStyle.Solid, 2.0);
            PlotData<DateTime, double> plotData2 = new PlotData<DateTime, double>(xData2, yData2, "Data", Color.Black, LineStyle.Solid, 2.0);
            PlotData<DateTime, double> plotData3 = new PlotData<DateTime, double>(xData3, yData3, "Data", Color.Black, LineStyle.Solid, 2.0);
            plotControl.PlotAxes = new ObservableCollection<Axis>() { bottomAxis, leftAxis };
            plotControl.PlotData = new ObservableCollection<object>() { plotData1 };
            plotControl.PlotData.Add(plotData2);
            plotControl.PlotData.Add(plotData3);

            csu.ShowControl(plotControl);

            Assert.Pass();
        }

        // Takes about 20 seconds to run
        //[Test, Apartment(System.Threading.ApartmentState.STA)]
        public void PlotALotOfData()
        {
            int n = 10000;
            int m = 10;

            List<List<DateTime>> xDataLists = new List<List<DateTime>>();
            List<List<double>> yDataLists = new List<List<double>>();

            Random random = new Random();

            for (int j = 1; j < m + 1; j++)
            {
                List<DateTime> x = new List<DateTime>();
                List<double> y = new List<double>();
                for (int i = 0; i < n; i++)
                {
                    x.Add(new DateTime(1, 1, 1).AddDays(i));
                    y.Add(random.Next(0, 100));
                }
                xDataLists.Add(x);
                yDataLists.Add(y);
            }

            string title = "Title";

            PlotControl plotControl = new PlotControl();
            plotControl.Title = title;

            Axis bottomAxis = new Axis("Bottom", Position.Bottom, AxisType.DateTime);
            Axis leftAxis = new Axis("Left", Position.Left, AxisType.Linear);

            plotControl.PlotAxes = new ObservableCollection<Axis>() { bottomAxis, leftAxis };
            plotControl.PlotData = new ObservableCollection<object>() { };

            for (int i = 0; i < m; i++)
            {
                plotControl.PlotData.Add(new PlotData<DateTime, double>(xDataLists[i], yDataLists[i], "Data", Color.Black, LineStyle.Solid, 2.0));
            }

            csu.ShowControl(plotControl);

            Assert.Pass();
        }
    }
}
