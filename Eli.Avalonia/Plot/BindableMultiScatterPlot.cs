using Avalonia;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Eli.Avalonia.Plot;

public partial class BindableMultiScatterPlot : AvaPlot
{
    public string Title { get; set; } = "";
    public static readonly DirectProperty<BindableMultiScatterPlot, string> TitleProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, string>(
        nameof(Title),
        o => o.Title,
        (o, v) => { o.Title = v; o.Plot.Title(v, true); });

    public string BottomAxis { get; set; } = "";
    public static readonly DirectProperty<BindableMultiScatterPlot, string> BottomAxisProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, string>(
        nameof(BottomAxis),
        o => o.BottomAxis,
        (o, v) => { o.BottomAxis = v; _ = o.Plot.XAxis.Label(v); });

    public string LeftAxis { get; set; } = "";
    public static readonly DirectProperty<BindableMultiScatterPlot, string> LeftAxisProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, string>(
        nameof(LeftAxis),
        o => o.LeftAxis,
        (o, v) => { o.LeftAxis = v; _ = o.Plot.YAxis.Label(v); });

    public string RightAxis { get; set; } = "";
    public static readonly DirectProperty<BindableMultiScatterPlot, string> RightAxisProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, string>(
        nameof(RightAxis),
        o => o.RightAxis,
        (o, v) => { o.RightAxis = v; _ = o.Plot.YAxis2.Label(v); });

    public ICollection<ScatterData> ScatterDataCollection { get; set; } = new List<ScatterData>();
    public static readonly DirectProperty<BindableMultiScatterPlot, ICollection<ScatterData>> ScatterDataCollectionProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, ICollection<ScatterData>>(
        nameof(ScatterDataCollection),
        o => o.ScatterDataCollection,
        (o, v) => o.ScatterDataCollection = v);

    private bool _isXDateTime = false;
    public bool IsXDateTime
    {
        get => _isXDateTime;
        set { Plot.XAxis.DateTimeFormat(value); _isXDateTime = value; }
    }
    public static readonly DirectProperty<BindableMultiScatterPlot, bool> IsXDateTimeProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, bool>(
        nameof(IsXDateTime),
        o => o.IsXDateTime,
        (o, v) => o.IsXDateTime = v);

    public bool RefreshDataToggle { get; set; } = false;
    public static readonly DirectProperty<BindableMultiScatterPlot, bool> RefreshDataToggleProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, bool>(
        nameof(RefreshDataToggle),
        o => o.RefreshDataToggle,
        (o, v) => { if(v == true) { o.RefreshData(); } o.RefreshDataToggle = false; });

    public string ErrorText { get; private set; } = "";
    public static readonly DirectProperty<BindableMultiScatterPlot, string> ErrorTextProperty = AvaloniaProperty.RegisterDirect<BindableMultiScatterPlot, string>(
        nameof(ErrorText),
        o => o.ErrorText);

    private readonly Color[] Colors = new Color[]
    {
        Color.Red,
        Color.Green,
        Color.Blue,
        Color.Orange,
        Color.Pink,
        Color.Indigo,
        Color.Violet,
        Color.Brown
    };

    public BindableMultiScatterPlot() : base()
    {
        Plot.YAxis2.IsVisible = true;
        Plot.YAxis2.Ticks(true);
        RefreshData();
    }

    public void RefreshData()
    {
        Plot.Clear();

        Plot.Style(ScottPlot.Style.Gray1);
        var darkBackground = System.Drawing.ColorTranslator.FromHtml("#2e3440");
        Plot.Style(figureBackground: darkBackground, dataBackground: darkBackground);

        ErrorText = "";
        for(var i = 0; i < ScatterDataCollection.Count; i++)
        {
            var scatterData = ScatterDataCollection.ElementAt(i);
            try
            {
                if(scatterData.XData.Length > 0)
                {
                    var plot = Plot.AddScatter(scatterData.XData, scatterData.YData, Colors[i % Colors.Length], label: scatterData.Label, markerSize: scatterData.MarkerSize);
                    plot.XAxisIndex = scatterData.XAxisIndex;
                    plot.YAxisIndex = scatterData.YAxisIndex;
                }
            }
            catch
            {
                ErrorText = $"Failed to plot item {i}.\n";
            }
        }
        _ = Plot.Legend();
        Refresh();
    }
}