using Avalonia;
using ScottPlot.TickGenerators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Eli.Avalonia.Plot;

public partial class BindableMultiScatterPlot : BindablePlot
{
    public ICollection<ScatterData> ScatterDataCollection { get; set; } = new List<ScatterData>();
    public static readonly DirectProperty<BindableMultiScatterPlot, ICollection<ScatterData>> ScatterDataCollectionProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, ICollection<ScatterData>>(
        nameof(ScatterDataCollection),
        o => o.ScatterDataCollection,
        (o, v) => o.ScatterDataCollection = v);

    private bool _isXDateTime = false;
    public bool IsXDateTime
    {
        get => _isXDateTime;
        set { if(value) _ = Plot.Axes.DateTimeTicksBottom(); _isXDateTime = value; }
    }
    public static readonly DirectProperty<BindableMultiScatterPlot, bool> IsXDateTimeProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, bool>(
        nameof(IsXDateTime),
        o => o.IsXDateTime,
        (o, v) => o.IsXDateTime = v);

    public bool XLog { get; set; } = false;
    public static readonly DirectProperty<BindableMultiScatterPlot, bool> XLogProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, bool>(
        nameof(XLog),
        o => o.XLog,
        (o, v) => o.XLog = v);

    public bool YLog { get; set; } = false;
    public static readonly DirectProperty<BindableMultiScatterPlot, bool> YLogProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, bool>(
        nameof(YLog),
        o => o.YLog,
        (o, v) => o.YLog = v);

    public int LogBase { get; set; } = 10;
    public static readonly DirectProperty<BindableMultiScatterPlot, int> LogBaseProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, int>(
        nameof(LogBase),
        o => o.LogBase,
        (o, v) => o.LogBase = v);

    private readonly Color[] Colors =
    [
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Orange,
        Color.Pink,
        Color.Indigo,
        Color.Violet,
        Color.Brown
    ];

    public BindableMultiScatterPlot() : base()
    {
        Plot.Axes.Right.IsVisible = true;
        Plot.Axes.Right.TickGenerator = new EmptyTickGenerator();
    }

    protected override void RefreshCustom()
    {
        ErrorText = "";
        for(var i = 0; i < ScatterDataCollection.Count; i++)
        {
            var scatterData = ScatterDataCollection.ElementAt(i);
            try
            {
                if(YLog)
                {
                    if(LogBase <= 0) LogBase = 10;
                    LogBase = 2;
                    string logTickLabels(double y) => Math.Pow(LogBase, y).ToString("N0");

                    LogMinorTickGenerator minorTickGen = new();
                    NumericAutomatic tickgen = new()
                    {
                        MinorTickGenerator = minorTickGen,
                        LabelFormatter = logTickLabels,
                        IntegerTicksOnly = true
                    };
                    Plot.Axes.Left.TickGenerator = tickgen;
                }
                else
                {
                    EvenlySpacedMinorTickGenerator minorTickGen = new(5.0);
                    NumericAutomatic tickgen = new()
                    {
                        MinorTickGenerator = minorTickGen,
                        LabelFormatter = (double y) => y.ToString(),
                        IntegerTicksOnly = false
                    };
                    Plot.Axes.Left.TickGenerator = tickgen;
                }

                if(scatterData.XData.Length > 0)
                {

                    var plot = Plot.Add.Scatter(scatterData.XData, YLog ? scatterData.YData.Select(y => System.Math.Log(y, LogBase)).ToArray() : scatterData.YData);
                    plot.LegendText = scatterData.Label;
                    plot.MarkerSize = scatterData.MarkerSize;
                    plot.LineWidth = scatterData.LineWidth;
                    plot.Color = ScottPlot.Color.FromColor(Colors[i % Colors.Length]);
                    //todo
                    //plot.XAxisIndex = scatterData.XAxisIndex;
                    //plot.YAxisIndex = scatterData.YAxisIndex;
                }
            }
            catch
            {
                ErrorText = $"Failed to plot item {i}.\n";
            }
        }
    }
}