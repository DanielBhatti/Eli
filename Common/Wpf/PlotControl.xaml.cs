using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using OxyPlot.Wpf;

namespace Common.Wpf
{
    /// <summary>
    /// Interaction logic for PlotControl.xaml
    /// </summary>
    public partial class PlotControl : OxyPlot.Wpf.Plot
    {
        public static readonly DependencyProperty PlotAxesProperty = DependencyProperty.Register(
            nameof(PlotAxes), typeof(ObservableCollection<Axis>), typeof(PlotControl),
            new PropertyMetadata(new PropertyChangedCallback(OnPlotAxisCollectionChanged)));
        public ObservableCollection<Axis> PlotAxes
        {
            get => (ObservableCollection<Axis>)GetValue(PlotAxesProperty);
            set
            {
                SetValue(PlotAxesProperty, value);
                UpdateAxes();
            }
        }

        public static readonly DependencyProperty PlotDataProperty = DependencyProperty.Register(
            nameof(PlotData), typeof(ObservableCollection<object>), typeof(PlotControl),
            new PropertyMetadata(new PropertyChangedCallback(OnPlotDataCollectionChanged)));
        public ObservableCollection<object> PlotData
        {
            get => (ObservableCollection<object>)GetValue(PlotDataProperty);
            set
            {
                SetValue(PlotDataProperty, value);
                UpdateData();
            }
        }

        public PlotControl()
        {
            InitializeComponent();
        }

        public void UpdateData()
        {
            Series.Clear();
            foreach (object plotData in PlotData)
            {
                if (plotData.GetType() == typeof(PlotData<double, double>))
                {
                    PlotData<double, double> datum = (PlotData<double, double>)plotData;
                    AddSeries(datum.XData, datum.YData, datum.Name, datum.Color, datum.MarkerSize, datum.LineStyle, datum.XAxisKey, datum.YAxisKey);
                }
                else if (plotData.GetType() == typeof(PlotData<DateTime, double>))
                {
                    PlotData<DateTime, double> datum = (PlotData<DateTime, double>)plotData;
                    AddSeries(datum.XData, datum.YData, datum.Name, datum.Color, datum.MarkerSize, datum.LineStyle, datum.XAxisKey, datum.YAxisKey);
                }
            }
            UpdatePlot();
        }

        public void UpdateAxes()
        {
            Axes.Clear();
            foreach (Axis axis in PlotAxes)
            {
                AddAxis(axis.Position, axis.Name, axis.AxisType);
            }
            UpdatePlot();
        }

        public void AddAxis(Position position, string name, AxisType axisType = AxisType.Linear)
        {
            OxyPlot.Wpf.Axis axis;
            switch (axisType)
            {
                case AxisType.Linear:
                    axis = new LinearAxis()
                    {
                        Title = name,
                        Name = name,
                        Key = name,
                        Position = position.ToOxyPlotAxisPosition(),
                    };
                    break;
                case AxisType.DateTime:
                    axis = new DateTimeAxis()
                    {
                        Title = name,
                        Name = name,
                        Key = name,
                        Position = position.ToOxyPlotAxisPosition(),
                        StringFormat = "yyyy-MM-dd"
                    };
                    break;
                case AxisType.Logarithmic:
                    axis = new LogarithmicAxis()
                    {
                        Title = name,
                        Name = name,
                        Key = name,
                        Position = position.ToOxyPlotAxisPosition()
                    };
                    break;
                default:
                    throw new NotImplementedException($"AxisType {axisType} is not implemented for use.");
            }
            Axes.Add(axis);
            UpdatePlot();
        }

        public void AddSeries<T1, T2>(IList<T1> xData, IList<T2> yData,
            string name = "", Color color = Color.Black, double markerSize = 1, LineStyle lineStyle = LineStyle.Solid,
            string xAxisKey = null, string yAxisKey = null)
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
                MarkerStroke = color.ToWindowsMediaColor(),
                XAxisKey = xAxisKey,
                YAxisKey = yAxisKey
            };

            if (typeof(T1) == typeof(double) && typeof(T2) == typeof(double))
            {
                lineSeries.ItemsSource = CollectionsToDataPoints((IList<double>)xData, (IList<double>)yData);
            }
            else if (typeof(T1) == typeof(DateTime) && typeof(T2) == typeof(double))
            {
                lineSeries.ItemsSource = CollectionsToDataPoints((IList<DateTime>)xData, (IList<double>)yData);
            }
            else throw new ArgumentException("Currently, only types T1 = double, DateTime and T2 = double are supported." +
                $"Provided arguments were T1 = {typeof(T1)} and T2 = {typeof(T2)}");

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

        private static void OnPlotAxisCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotControl control = (PlotControl)d;
            if (e.OldValue != null)
            {
                var coll = (INotifyCollectionChanged)e.OldValue;
                coll.CollectionChanged -= control.PlotAxisCollectionChanged;
            }

            if (e.NewValue != null)
            {
                var coll = (ObservableCollection<Axis>)e.NewValue;

                coll.CollectionChanged += control.PlotAxisCollectionChanged;
            }
        }

        private void PlotAxisCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PlotAxes = PlotAxes;
        }

        private static void OnPlotDataCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlotControl control = (PlotControl)d;
            if (e.OldValue != null)
            {
                var coll = (INotifyCollectionChanged)e.OldValue;
                coll.CollectionChanged -= control.PlotDataCollectionChanged;
            }

            if (e.NewValue != null)
            {
                var coll = (ObservableCollection<object>)e.NewValue;

                coll.CollectionChanged += control.PlotDataCollectionChanged;
            }
        }

        private void PlotDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PlotData = PlotData;
        }
    }
}
