using Avalonia;
using ScottPlot.Avalonia;
using System.Collections.Generic;
using System.Linq;

namespace Eli.Avalonia.Plot;

public class BindableHistogram : AvaPlot
{
    public string Title { get; set; } = "";
    public static readonly DirectProperty<BindableHistogram, string> TitleProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, string>(
        nameof(Title),
        o => o.Title,
        (o, v) => { o.Title = v; o.Plot.Title(v, true); });

    public string BottomAxis { get; set; } = "";
    public static readonly DirectProperty<BindableHistogram, string> BottomAxisProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, string>(
        nameof(BottomAxis),
        o => o.BottomAxis,
        (o, v) => { o.BottomAxis = v; _ = o.Plot.XAxis.Label(v); });

    public string LeftAxis { get; set; } = "";
    public static readonly DirectProperty<BindableHistogram, string> LeftAxisProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, string>(
        nameof(LeftAxis),
        o => o.LeftAxis,
        (o, v) => { o.LeftAxis = v; _ = o.Plot.YAxis.Label(v); });

    public string RightAxis { get; set; } = "";
    public static readonly DirectProperty<BindableHistogram, string> RightAxisProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, string>(
        nameof(RightAxis),
        o => o.RightAxis,
        (o, v) => { o.RightAxis = v; _ = o.Plot.YAxis2.Label(v); });

    public ICollection<double> Data { get; set; } = new List<double>();
    public static readonly DirectProperty<BindableHistogram, ICollection<double>> DataProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, ICollection<double>>(
        nameof(Data),
        o => o.Data,
        (o, v) => o.Data = v);

    public bool RefreshDataToggle { get; set; } = false;
    public static readonly DirectProperty<BindableHistogram, bool> RefreshDataToggleProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, bool>(
        nameof(RefreshDataToggle),
        o => o.RefreshDataToggle,
        (o, v) => { if(v == true) { o.RefreshData(); } o.RefreshDataToggle = false; });

    public string ErrorText { get; private set; } = "";
    public static readonly DirectProperty<BindableMultiScatterPlot, string> ErrorTextProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, string>(
        nameof(ErrorText),
        o => o.ErrorText);

    public BindableHistogram() : base()
    {
        Plot.YAxis2.IsVisible = true;
        Plot.YAxis2.Ticks(true);
        RefreshData();
    }

    public void RefreshData()
    {
        if(Data.Count == 0) return;
        Plot.Clear();
        Plot.Style(ScottPlot.Style.Gray1);
        var darkBackground = System.Drawing.ColorTranslator.FromHtml("#2e3440");
        Plot.Style(figureBackground: darkBackground, dataBackground: darkBackground);
        ErrorText = "";
        var histogram = new ScottPlot.Statistics.Histogram(Data.Min(), Data.Max(), 100);
        histogram.AddRange(Data);

        var bars = Plot.AddBar(histogram.Counts, histogram.Bins);
        bars.BarWidth = 0.2;
        Plot.Title("Histogram");
        Plot.XLabel("Value");
        Plot.YLabel("Frequency");
        Plot.SetAxisLimits(yMin: 0);
        Refresh();
    }
}
