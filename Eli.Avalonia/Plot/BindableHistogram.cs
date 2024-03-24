using Avalonia;
using Avalonia.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Eli.Avalonia.Plot;

public class BindableHistogram : BindablePlot
{
    public ICollection<(double, double)> CurveData { get; set; } = new List<(double, double)>();
    public static readonly DirectProperty<BindableHistogram, ICollection<(double, double)>> CurveDataProperty =
        AvaloniaProperty.RegisterDirect<BindableHistogram, ICollection<(double, double)>>(
            nameof(CurveData),
            o => o.CurveData,
            (o, v) => o.CurveData = v);

    private ICollection<double> _bins = new AvaloniaList<double>();
    public ICollection<double> Bins
    {
        get => _bins;
        private set => SetAndRaise(BinsProperty, ref _bins, value);
    }
    public static readonly DirectProperty<BindableHistogram, ICollection<double>> BinsProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, ICollection<double>>(
        nameof(Bins),
        o => o.Bins);

    private ICollection<double> _counts = new AvaloniaList<double>();
    public ICollection<double> Counts
    {
        get => _counts;
        private set => SetAndRaise(CountsProperty, ref _counts, value);
    }
    public static readonly DirectProperty<BindableHistogram, ICollection<double>> CountsProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, ICollection<double>>(
        nameof(Counts),
        o => o.Counts);

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

        Counts = new AvaloniaList<double>(histogram.Counts);
        Bins = new AvaloniaList<double>(histogram.Bins);

        if(IsCumulativeProbability) _ = Plot.AddScatterStep(xs: histogram.Bins, ys: histogram.GetCumulativeProbability());
        else
        {
            var bars = Plot.AddBar(histogram.Counts, histogram.Bins);
            bars.BarWidth = BinWidth ?? (histogram.Bins.Max() - histogram.Bins.Min()) / NumberOfBins;
        }

        if(CurveData?.Count() > 0) _ = Plot.AddScatter(CurveData.Select(p => p.Item1).ToArray(), CurveData.Select(p => p.Item2).ToArray());

        Plot.Title("Histogram");
        Plot.XLabel("Value");
        Plot.YLabel("Frequency");
        Plot.SetAxisLimits(yMin: 0);
    }
}
