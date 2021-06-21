using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot.Wpf;

namespace Common.Wpf
{
    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : OxyPlot.Wpf.Plot
    {
        public PlotControl()
        {
            InitializeComponent();
        }

        public void AddAxis(Position position, string name, AxisType axisType = AxisType.Linear)
        {
            Axis axis;
            switch (axisType)
            {
                case AxisType.Linear:
                    axis = new LinearAxis()
                    {
                        Title = name,
                        Name = name,
                        Position = position.ToOxyPlotAxisPosition(),
                    };
                    break;
                case AxisType.DateTime:
                    axis = new DateTimeAxis()
                    {
                        Title = name,
                        Name = name,
                        Position = position.ToOxyPlotAxisPosition(),
                        StringFormat = "yyyy-mm-dd"
                    };
                    break;
                case AxisType.Logarithmic:
                    axis = new LogarithmicAxis()
                    {
                        Title = name,
                        Name = name,
                        Position = position.ToOxyPlotAxisPosition()
                    };
                    break;
                default:
                    throw new NotImplementedException($"AxisType {axisType} is not implemented for use.");
            }
            Axes.Add(axis);
            UpdatePlot();
        }

        public void AddSeries(IList<double> xData, IList<double> yData,
            string name = "", Color color = Color.Black, double markerSize = 2, LineStyle lineStyle = LineStyle.Solid)
        {
            if (xData.Count != yData.Count) throw new ArgumentException($"Collections must be the same length, {nameof(xData)} was {xData.Count} and {nameof(yData)} was {yData.Count}.");

            LineSeries lineSeries = new LineSeries()
            {
                Title = name,
                Name = name,
                Color = color.ToWindowsMediaColor(),
                MarkerSize = markerSize,
                LineStyle = lineStyle.ToOxyPlotLineStyle(),
                MarkerType = OxyPlot.MarkerType.Circle,
                MarkerFill = color.ToWindowsMediaColor(),
                MarkerStroke = color.ToWindowsMediaColor()
            };
            lineSeries.ItemsSource = CollectionsToDataPoints(xData, yData);

            Series.Add(lineSeries);
            UpdatePlot();
        }

        public void AddSeries(IList<DateTime> xData, IList<double> yData,
            string name = "", Color color = Color.Black, double markerSize = 2, LineStyle lineStyle = LineStyle.Solid)
        {
            if (xData.Count != yData.Count) throw new ArgumentException($"Collections must be the same length, {nameof(xData)} was {xData.Count} and {nameof(yData)} was {yData.Count}.");

            LineSeries lineSeries = new LineSeries()
            {
                Title = name,
                Name = name,
                Color = color.ToWindowsMediaColor(),
                MarkerSize = markerSize,
                LineStyle = lineStyle.ToOxyPlotLineStyle(),
                MarkerType = OxyPlot.MarkerType.Circle,
                MarkerFill = color.ToWindowsMediaColor(),
                MarkerStroke = color.ToWindowsMediaColor()
            };
            lineSeries.ItemsSource = CollectionsToDataPoints(xData, yData);

            Series.Add(lineSeries);
            UpdatePlot();
        }

        private void UpdatePlot()
        {
            InvalidatePlot(true);
        }

        private List<OxyPlot.DataPoint> CollectionsToDataPoints(IList<double> xData, IList<double> yData)
        {
            List<OxyPlot.DataPoint> dataPoints = new List<OxyPlot.DataPoint>();
            for (int i = 0; i < xData.Count; i++)
            {
                dataPoints.Add(new OxyPlot.DataPoint(xData[i], yData[i]));
            }
            return dataPoints;
        }

        private List<OxyPlot.DataPoint> CollectionsToDataPoints(IList<DateTime> xData, IList<double> yData)
        {
            List<OxyPlot.DataPoint> dataPoints = new List<OxyPlot.DataPoint>();
            for (int i = 0; i < xData.Count; i++)
            {
                double dateTimeDouble = OxyPlot.Axes.DateTimeAxis.ToDouble(xData[i]);
                dataPoints.Add(new OxyPlot.DataPoint(dateTimeDouble, yData[i]));
            }
            return dataPoints;
        }
    }
}
