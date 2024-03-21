using Avalonia;
using System.Collections.Generic;
using System.Linq;

namespace Eli.Avalonia.Plot;

public class BindableHistogram : BindablePlot
{
    public ICollection<double> Data { get; set; } = new List<double>();
    public static readonly DirectProperty<BindableHistogram, ICollection<double>> DataProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, ICollection<double>>(
        nameof(Data),
        o => o.Data,
        (o, v) => o.Data = v);

    public int NumberOfBins { get; set; } = 10;
    public static readonly DirectProperty<BindableHistogram, int> NumberOfBinsProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, int>(
        nameof(NumberOfBins),
        o => o.NumberOfBins,
        (o, v) => o.NumberOfBins = v);

    public int? BinWidth { get; set; } = null;
    public static readonly DirectProperty<BindableHistogram, int?> BinWidthProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, int?>(
        nameof(BinWidth),
        o => o.BinWidth,
        (o, v) => o.BinWidth = v);

    public bool IsCumulativeProbability { get; set; } = false;
    public static readonly DirectProperty<BindableHistogram, bool> IsCumulativeProbabilityProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, bool>(
        nameof(IsCumulativeProbability),
        o => o.IsCumulativeProbability);

    public BindableHistogram() : base() { }

    protected override void RefreshCustom()
    {
        if(Data.Count == 0 || NumberOfBins == 0) return;
        
        var histogram = new ScottPlot.Statistics.Histogram(Data.Min(), Data.Max(), NumberOfBins);
        histogram.AddRange(Data);

        if(IsCumulativeProbability) _ = Plot.AddScatterStep(xs: histogram.Bins, ys: histogram.GetCumulativeProbability());
        else
        {
            var bars = Plot.AddBar(histogram.Counts, histogram.Bins);
            bars.BarWidth = BinWidth ?? (histogram.Bins.Max() - histogram.Bins.Min()) / NumberOfBins;
        }
        Plot.Title("Histogram");
        Plot.XLabel("Value");
        Plot.YLabel("Frequency");
        Plot.SetAxisLimits(yMin: 0);
    }
}
