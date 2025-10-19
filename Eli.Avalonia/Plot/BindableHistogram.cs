using Avalonia;
using Avalonia.Collections;
using System;
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

    private IEnumerable<double> _counts = new AvaloniaList<double>();
    public IEnumerable<double> Counts
    {
        get => _counts;
        private set => SetAndRaise(CountsProperty, ref _counts, value);
    }
    public static readonly DirectProperty<BindableHistogram, IEnumerable<double>> CountsProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, IEnumerable<double>>(
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

    public bool IsNormalizedProbability { get; set; } = false;
    public static readonly DirectProperty<BindableHistogram, bool> IsNormalizedProbabilityProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, bool>(
    nameof(IsNormalizedProbability),
    o => o.IsNormalizedProbability,
    (o, v) => o.IsNormalizedProbability = v);

    public string Label { get; protected set; } = "";
    public static readonly DirectProperty<BindableHistogram, string> LabelProperty = AvaloniaProperty.RegisterDirect<BindableHistogram, string>(
        nameof(Label),
        o => o.Label,
        (o, v) => o.Label = v);

    public BindableHistogram() : base() { }

    protected override void RefreshCustom()
    {
        if(Data.Count == 0 || NumberOfBins == 0) return;

        var min = Data.Min();
        var max = Data.Max();

        if(min == max) return;

        var histogram = ScottPlot.Statistics.Histogram.WithBinCount(NumberOfBins, min, max);
        histogram.AddRange(Data);

        if(IsCumulativeProbability)
        {
            var scatterPlot = Plot.Add.ScatterPoints(xs: histogram.Bins, ys: histogram.GetCumulativeProbability());
            scatterPlot.LegendText = Label;
            Counts = histogram.GetCumulativeProbability();
            Bins = histogram.Bins;
        }
        else
        {
            var counts = IsNormalizedProbability ? histogram.GetProbability() : histogram.Counts.Select(Convert.ToDouble);
            var bars = Plot.Add.Bars(counts, histogram.Bins);
            bars.LegendText = Label;
            Counts = counts;
            Bins = histogram.Bins;
        }

        if(CurveData?.Count > 0 && CurveData.All(cd => !double.IsNaN(cd.Item1) && !double.IsNaN(cd.Item2))) _ = Plot.Add.Scatter(CurveData.Select(p => p.Item1).ToArray(), CurveData.Select(p => p.Item2).ToArray());

        Plot.Title("Histogram");
        Plot.XLabel("Value");
        Plot.YLabel("Frequency");
        //todo
        //Plot.SetAxisLimits(yMin: 0);
    }

    private static double AutoBinWidth(ICollection<double> data)
    {
        var n = data.Count;
        if(n < 2) return 1;

        var sorted = data.OrderBy(v => v).ToArray();
        var q1 = PercentileSorted(sorted, 0.25);
        var q3 = PercentileSorted(sorted, 0.75);
        var iqr = q3 - q1;
        var fd = iqr > 0 ? 2.0 * iqr / Math.Cbrt(n) : double.NaN; // 2 * IQR / n^(1/3)

        var mean = sorted.Average();
        var sd = Math.Sqrt(sorted.Select(v => (v - mean) * (v - mean)).Sum() / (n - 1));
        var scott = sd > 0 ? 3.49 * sd / Math.Cbrt(n) : double.NaN; // 3.49 * σ / n^(1/3)

        var raw = double.IsFinite(fd) && fd > 0 ? fd : (double.IsFinite(scott) && scott > 0 ? scott :
                      Math.Max((sorted[^1] - sorted[0]) / Math.Max(10, Math.Min(50, n)), 1e-9));

        return NiceStep(raw);
    }

    private static double PercentileSorted(IList<double> sorted, double p)
    {
        if(sorted.Count == 0) return double.NaN;
        var pos = (sorted.Count - 1) * p;
        var lo = (int)Math.Floor(pos);
        var hi = (int)Math.Ceiling(pos);
        if(lo == hi) return sorted[lo];
        var frac = pos - lo;
        return sorted[lo] + (sorted[hi] - sorted[lo]) * frac;
    }

    private static double NiceStep(double x)
    {
        if(!(x > 0) || double.IsNaN(x) || double.IsInfinity(x)) return 1;
        var exp = Math.Floor(Math.Log10(x));
        var frac = x / Math.Pow(10, exp);
        return ((frac <= 1) ? 1 : (frac <= 2) ? 2 : (frac <= 5) ? 5 : 10) * Math.Pow(10, exp);
    }
}
