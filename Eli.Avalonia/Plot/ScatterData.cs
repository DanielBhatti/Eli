using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eli.Avalonia.Plot;

public class ScatterData : AvaloniaObject
{
    public int XAxisIndex { get; set; } = 0;
    public static readonly DirectProperty<ScatterData, int> XAxisIndexProperty = AvaloniaProperty.RegisterDirect<ScatterData, int>(
        nameof(XData),
        o => o.XAxisIndex,
        (o, v) => o.XAxisIndex = v);

    public int YAxisIndex { get; set; } = 0;
    public static readonly DirectProperty<ScatterData, int> YAxisIndexProperty = AvaloniaProperty.RegisterDirect<ScatterData, int>(
        nameof(YData),
        o => o.YAxisIndex,
        (o, v) => o.YAxisIndex = v);

    public double[] XData { get; set; } = [];
    public static readonly DirectProperty<ScatterData, double[]> XDataProperty = AvaloniaProperty.RegisterDirect<ScatterData, double[]>(
        nameof(XData),
        o => o.XData,
        (o, v) => o.XData = v);

    public double[] YData { get; set; } = [];
    public static readonly DirectProperty<ScatterData, double[]> YDataProperty = AvaloniaProperty.RegisterDirect<ScatterData, double[]>(
        nameof(YData),
        o => o.YData,
        (o, v) => o.YData = v);

    public string Label { get; set; }
    public static readonly DirectProperty<ScatterData, string> LabelProperty = AvaloniaProperty.RegisterDirect<ScatterData, string>(
        nameof(Label),
        o => o.Label,
        (o, v) => o.Label = v);

    public float MarkerSize { get; set; }
    public static readonly DirectProperty<ScatterData, float> MarkerSizeProperty = AvaloniaProperty.RegisterDirect<ScatterData, float>(
        nameof(MarkerSize),
        o => o.MarkerSize,
        (o, v) => o.MarkerSize = v);

    public int LineWidth { get; set; }
    public static readonly DirectProperty<ScatterData, int> LineWidthProperty = AvaloniaProperty.RegisterDirect<ScatterData, int>(
        nameof(LineWidth),
        o => o.LineWidth,
        (o, v) => o.LineWidth = v);

    public ScatterData(IEnumerable<decimal> xData, IEnumerable<decimal> yData, int xAxisIndex = 0, int yAxisIndex = 0, string label = "", float markerSize = 3, int lineWidth = 1) : this(xData.Select(Convert.ToDouble), yData.Select(Convert.ToDouble), xAxisIndex, yAxisIndex, label, markerSize, lineWidth) { }
    public ScatterData(IEnumerable<double> xData, IEnumerable<double> yData, int xAxisIndex = 0, int yAxisIndex = 0, string label = "", float markerSize = 3, int lineWidth = 1)
    {
        XData = xData.ToArray();
        YData = yData.ToArray();
        XAxisIndex = xAxisIndex;
        YAxisIndex = yAxisIndex;
        Label = label;
        MarkerSize = markerSize;
        LineWidth = lineWidth;
    }
}
