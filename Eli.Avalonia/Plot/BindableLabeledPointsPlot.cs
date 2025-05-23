using Avalonia;
using System.Linq;

namespace Eli.Avalonia.Plot;

public class BindableLabeledPointsPlot : BindablePlot
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
    public static readonly DirectProperty<BindableLabeledPointsPlot, double[]> XDataProperty = AvaloniaProperty.RegisterDirect<BindableLabeledPointsPlot, double[]>(
        nameof(XData),
        o => o.XData,
        (o, v) => o.XData = v);

    public double[] YData { get; set; } = [];
    public static readonly DirectProperty<BindableLabeledPointsPlot, double[]> YDataProperty = AvaloniaProperty.RegisterDirect<BindableLabeledPointsPlot, double[]>(
        nameof(YData),
        o => o.YData,
        (o, v) => o.YData = v);

    public string[] Labels { get; set; } = [];
    public static readonly DirectProperty<BindableLabeledPointsPlot, string[]> LabelsProperty = AvaloniaProperty.RegisterDirect<BindableLabeledPointsPlot, string[]>(
        nameof(Labels),
        o => o.Labels,
        (o, v) => o.Labels = v);

    protected override void RefreshCustom()
    {
        if(_scatterPlot is not null) Plot.Remove(_scatterPlot);
        if(XData is not null && YData is not null && XData.Length > 0 && XData.Length == YData.Length)
        {
            _scatterPlot = Plot.Add.ScatterPoints(XData, YData);
            if(Labels is not null && Labels.Length == XData.Length)
            {
                for(var i = 0; i < Labels.Length; i++) _ = Plot.Add.Text(Labels[i], XData[i], YData[i]);
            }
        }
    }
}
