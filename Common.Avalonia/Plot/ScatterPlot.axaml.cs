using Avalonia;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace Common.Avalonia.Plot;

public partial class ScatterPlot : AvaPlot
{
    private ScottPlot.Plottable.ScatterPlot? _scatterPlot;

    public string Title { get; set; } = "";
    public static readonly DirectProperty<ScatterPlot, string> TitleProperty = AvaloniaProperty.RegisterDirect<ScatterPlot, string>(
        nameof(Title),
        o => o.Title,
        (o, v) => { o.Title = v; o.Plot.Title(v, true); });

    public string XAxis { get; set; } = "";
    public static readonly DirectProperty<ScatterPlot, string> XAxisProperty = AvaloniaProperty.RegisterDirect<ScatterPlot, string>(
        nameof(XAxis),
        o => o.XAxis,
        (o, v) => { o.XAxis = v; _ = o.Plot.XAxis.Label(v); });

    public string YAxis { get; set; } = "";
    public static readonly DirectProperty<ScatterPlot, string> YAxisProperty = AvaloniaProperty.RegisterDirect<ScatterPlot, string>(
        nameof(YAxis),
        o => o.YAxis,
        (o, v) => { o.YAxis = v; _ = o.Plot.YAxis.Label(v); });

    public double[] XData { get; set; } = System.Array.Empty<double>();
    public static readonly DirectProperty<ScatterPlot, double[]> XDataProperty = AvaloniaProperty.RegisterDirect<ScatterPlot, double[]>(
        nameof(XData),
        o => o.XData,
        (o, v) => { o.XData = v; o.PlotData(); });

    public double[] YData { get; set; } = System.Array.Empty<double>();
    public static readonly DirectProperty<ScatterPlot, double[]> YDataProperty = AvaloniaProperty.RegisterDirect<ScatterPlot, double[]>(
        nameof(YData),
        o => o.YData,
        (o, v) => { o.YData = v; o.PlotData(); });

    private bool _isXDateTime = false;
    public bool IsXDateTime
    {
        get => _isXDateTime;
        set { Plot.XAxis.DateTimeFormat(value); _isXDateTime = value; }
    }
    public static readonly DirectProperty<MultiScatterPlot, bool> IsXDateTimeProperty = AvaloniaProperty.RegisterDirect<MultiScatterPlot, bool>(
        nameof(IsXDateTime),
        o => o.IsXDateTime,
        (o, v) => o.IsXDateTime = v);

    public ScatterPlot() : base() => InitializeComponent();

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

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