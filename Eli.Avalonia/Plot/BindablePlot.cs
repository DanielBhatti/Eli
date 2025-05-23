using Avalonia;
using ScottPlot;
using ScottPlot.Avalonia;

namespace Eli.Avalonia.Plot;

public abstract class BindablePlot : AvaPlot
{
    public string Title { get; set; } = "";
    public static readonly DirectProperty<BindablePlot, string> TitleProperty = AvaloniaProperty.RegisterDirect<BindablePlot, string>(
        nameof(Title),
        o => o.Title,
        (o, v) => { o.Title = v; o.Plot.Title(v); });

    public string BottomAxis { get; set; } = "";
    public static readonly DirectProperty<BindablePlot, string> BottomAxisProperty = AvaloniaProperty.RegisterDirect<BindablePlot, string>(
        nameof(BottomAxis),
        o => o.BottomAxis,
        (o, v) => { o.BottomAxis = v; o.Plot.XLabel(v); });

    public string LeftAxis { get; set; } = "";
    public static readonly DirectProperty<BindablePlot, string> LeftAxisProperty = AvaloniaProperty.RegisterDirect<BindablePlot, string>(
        nameof(LeftAxis),
        o => o.LeftAxis,
        (o, v) => { o.LeftAxis = v; o.Plot.YLabel(v); });

    public string RightAxis { get; set; } = "";
    public static readonly DirectProperty<BindablePlot, string> RightAxisProperty = AvaloniaProperty.RegisterDirect<BindablePlot, string>(
        nameof(RightAxis),
        o => o.RightAxis,
        (o, v) => { o.RightAxis = v; o.Plot.Axes.Right.Label.Text = v; });

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
        var darkBackground = ScottPlot.Color.FromColor(System.Drawing.ColorTranslator.FromHtml("#383a3f"));
        Plot.FigureBackground.Color = darkBackground;
        Plot.DataBackground.Color = darkBackground.Darken(0.1);
        Plot.Grid.MajorLineColor = darkBackground.Lighten(0.3);

        var defaultLabelStyle = new LabelStyle
        {
            BackgroundColor = darkBackground,
            ForeColor = darkBackground.Lighten(0.8),
            FontSize = 13
        };
        Plot.Axes.Bottom.TickLabelStyle = defaultLabelStyle;
        Plot.Axes.Left.TickLabelStyle = defaultLabelStyle;
        Plot.Axes.Color(darkBackground.Lighten(0.8));
        if(IsShowingLegend) _ = Plot.ShowLegend();
    }

    private void RefreshPlot()
    {
        Plot.Clear();
        ApplyStyles();
        RefreshCustom();
        base.Refresh();
        Plot.Axes.AutoScale();
    }
}
