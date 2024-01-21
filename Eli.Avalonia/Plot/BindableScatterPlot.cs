using Avalonia;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace Eli.Avalonia.Plot;

public partial class BindableScatterPlot : AvaPlot
{
    private ScottPlot.Plottable.ScatterPlot? _scatterPlot;

    public string Title { get; set; } = "";
    public static readonly DirectProperty<BindableScatterPlot, string> TitleProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, string>(
        nameof(Title),
        o => o.Title,
        (o, v) => { o.Title = v; o.Plot.Title(v, true); });

    public string XAxis { get; set; } = "";
    public static readonly DirectProperty<BindableScatterPlot, string> XAxisProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, string>(
        nameof(XAxis),
        o => o.XAxis,
        (o, v) => { o.XAxis = v; _ = o.Plot.XAxis.Label(v); });

    public string YAxis { get; set; } = "";
    public static readonly DirectProperty<BindableScatterPlot, string> YAxisProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, string>(
        nameof(YAxis),
        o => o.YAxis,
        (o, v) => { o.YAxis = v; _ = o.Plot.YAxis.Label(v); });

    public double[] XData { get; set; } = System.Array.Empty<double>();
    public static readonly DirectProperty<BindableScatterPlot, double[]> XDataProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, double[]>(
        nameof(XData),
        o => o.XData,
        (o, v) => { o.XData = v; o.PlotData(); });

    public double[] YData { get; set; } = System.Array.Empty<double>();
    public static readonly DirectProperty<BindableScatterPlot, double[]> YDataProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, double[]>(
        nameof(YData),
        o => o.YData,
        (o, v) => { o.YData = v; o.PlotData(); });

    private bool _isXDateTime = false;
    public bool IsXDateTime
    {
        get => _isXDateTime;
        set { Plot.XAxis.DateTimeFormat(value); _isXDateTime = value; }
    }
    public static readonly DirectProperty<BindableScatterPlot, bool> IsXDateTimeProperty = AvaloniaProperty.RegisterDirect<BindableScatterPlot, bool>(
        nameof(IsXDateTime),
        o => o.IsXDateTime,
        (o, v) => o.IsXDateTime = v);

    public BindableScatterPlot() : base() { }

    private void PlotData()
    {
        if(_scatterPlot is not null) Plot.Remove(_scatterPlot);

        Plot.Style(ScottPlot.Style.Gray1);
        var darkBackground = System.Drawing.ColorTranslator.FromHtml("#2e3440");
        Plot.Style(figureBackground: darkBackground, dataBackground: darkBackground);

        if(XData is not null && YData is not null && XData.Length == YData.Length) _scatterPlot = XData.Length <= 1000 ? Plot.AddScatter(XData, YData) : Plot.AddScatter(XData, YData, markerSize: 0);
        Refresh();
    }
}