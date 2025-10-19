using Avalonia;

namespace Eli.Avalonia.Plot;

public partial class BindableScatterPlot : BindablePlot
{
    private ScottPlot.Plottables.Scatter? _scatterPlot;

    public string XAxis { get; set; } = "";
    public static readonly DirectProperty<BindableScatterPlot, string> XAxisProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, string>(
        nameof(XAxis),
        o => o.XAxis,
        (o, v) => { o.XAxis = v; o.Plot.XLabel(v); });

    public string YAxis { get; set; } = "";
    public static readonly DirectProperty<BindableScatterPlot, string> YAxisProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, string>(
        nameof(YAxis),
        o => o.YAxis,
        (o, v) => { o.YAxis = v; o.Plot.YLabel(v); });

    public double[] XData { get; set; } = [];
    public static readonly DirectProperty<BindableScatterPlot, double[]> XDataProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, double[]>(
        nameof(XData),
        o => o.XData,
        (o, v) => { o.XData = v; o.RefreshCustom(); });

    public double[] YData { get; set; } = [];
    public static readonly DirectProperty<BindableScatterPlot, double[]> YDataProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, double[]>(
        nameof(YData),
        o => o.YData,
        (o, v) => { o.YData = v; o.RefreshCustom(); });

    private bool _isXDateTime = false;
    public bool IsXDateTime
    {
        get => _isXDateTime;
        set { if(value) _ = Plot.Axes.DateTimeTicksBottom(); _isXDateTime = value; }
    }
    public static readonly DirectProperty<BindableScatterPlot, bool> IsXDateTimeProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, bool>(
        nameof(IsXDateTime),
        o => o.IsXDateTime,
        (o, v) => o.IsXDateTime = v);

    public int LineWidth { get; set; } = 1;
    public static readonly DirectProperty<BindableScatterPlot, int> LineWidthProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, int>(
        nameof(LineWidth),
        o => o.LineWidth,
        (o, v) => o.LineWidth = v);

    public BindableScatterPlot() : base() { }

    protected override void RefreshCustom()
    {
        if(_scatterPlot is not null) Plot.Remove(_scatterPlot);
        if(XData is not null && YData is not null && XData.Length > 0 && XData.Length == YData.Length)
        {
            _scatterPlot = Plot.Add.Scatter(XData, YData);
            _scatterPlot.LineWidth = LineWidth;
            if(XData.Length > 1000) _scatterPlot.MarkerSize = 0;
        }
    }
}