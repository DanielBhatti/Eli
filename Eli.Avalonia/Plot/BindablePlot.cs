using Avalonia;
using ScottPlot.Avalonia;

namespace Eli.Avalonia.Plot;

public abstract class BindablePlot : AvaPlot
{
    public string Title { get; set; } = "";
    public static readonly DirectProperty<BindablePlot, string> TitleProperty = AvaloniaProperty.RegisterDirect<BindablePlot, string>(
        nameof(Title),
        o => o.Title,
        (o, v) => { o.Title = v; o.Plot.Title(v, true); });

    public string BottomAxis { get; set; } = "";
    public static readonly DirectProperty<BindablePlot, string> BottomAxisProperty = AvaloniaProperty.RegisterDirect<BindablePlot, string>(
        nameof(BottomAxis),
        o => o.BottomAxis,
        (o, v) => { o.BottomAxis = v; _ = o.Plot.XAxis.Label(v); });

    public string LeftAxis { get; set; } = "";
    public static readonly DirectProperty<BindablePlot, string> LeftAxisProperty = AvaloniaProperty.RegisterDirect<BindablePlot, string>(
        nameof(LeftAxis),
        o => o.LeftAxis,
        (o, v) => { o.LeftAxis = v; _ = o.Plot.YAxis.Label(v); });

    public string RightAxis { get; set; } = "";
    public static readonly DirectProperty<BindablePlot, string> RightAxisProperty = AvaloniaProperty.RegisterDirect<BindablePlot, string>(
        nameof(RightAxis),
        o => o.RightAxis,
        (o, v) => { o.RightAxis = v; _ = o.Plot.YAxis2.Label(v); });

    public bool RefreshDataToggle { get; set; } = false;
    public static readonly DirectProperty<BindablePlot, bool> RefreshDataToggleProperty = AvaloniaProperty.RegisterDirect<BindablePlot, bool>(
        nameof(RefreshDataToggle),
        o => o.RefreshDataToggle,
        (o, v) =>
        {
            if(v == true) o.RefreshPlot();
            o.RefreshDataToggle = false;
        });

    public string ErrorText { get; protected set; } = "";
    public static readonly DirectProperty<BindablePlot, string> ErrorTextProperty = AvaloniaProperty.RegisterDirect<BindablePlot, string>(
        nameof(ErrorText),
        o => o.ErrorText);

    public bool IsShowingLegend { get; set; } = true;
    public static readonly DirectProperty<BindablePlot, bool> IsShowingLegendProperty = AvaloniaProperty.RegisterDirect<BindablePlot, bool>(
        nameof(IsShowingLegend),
        o => o.IsShowingLegend,
        (o, v) => o.IsShowingLegend = v);

    public BindablePlot() : base() => RefreshPlot();

    protected abstract void RefreshCustom();
    protected virtual void ApplyStyles()
    {
        Plot.Style(ScottPlot.Style.Gray1);
        var darkBackground = System.Drawing.ColorTranslator.FromHtml("#2e3440");
        Plot.Style(figureBackground: darkBackground, dataBackground: darkBackground);
        Plot.XAxis.TickLabelStyle(color: System.Drawing.Color.White, fontSize: 13);
        Plot.YAxis.TickLabelStyle(color: System.Drawing.Color.White, fontSize: 13);
        if(IsShowingLegend) _ = Plot.Legend();
    }

    private void RefreshPlot()
    {
        Plot.Clear();
        ApplyStyles();
        RefreshCustom();
        base.Refresh();
    }
}
