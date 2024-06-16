using Avalonia;
using System.Linq;

namespace Eli.Avalonia.Plot;

public class BindableLabeledPointsPlot : BindablePlot
{
    private ScottPlot.Plottable.ScatterPlot? _scatterPlot;

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
            _scatterPlot = XData.Length <= 1000 ? Plot.AddScatter(XData, YData, lineWidth: 0) : Plot.AddScatter(XData, YData, markerSize: 0, lineWidth: 0);
            if(Labels is not null && Labels.Length == XData.Length)
            {
                for(var i = 0; i < Labels.Length; i++) _ = Plot.AddText(Labels[i], XData[i], YData[i]);
            }
        }
    }
}
