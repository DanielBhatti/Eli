using Avalonia;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using ScottPlot.Renderable;
using Avalonia.Data;

namespace Common.Avalonia.Plot
{
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
            (o, v) => { o.XAxis = v; o.Plot.XAxis.Label(v); });

        public string YAxis { get; set; } = "";
        public static readonly DirectProperty<ScatterPlot, string> YAxisProperty = AvaloniaProperty.RegisterDirect<ScatterPlot, string>(
            nameof(YAxis),
            o => o.YAxis,
            (o, v) => { o.YAxis = v; o.Plot.YAxis.Label(v); });

        public double[] XData { get; set; } = new double[0];
        public static readonly DirectProperty<ScatterPlot, double[]> XDataProperty = AvaloniaProperty.RegisterDirect<ScatterPlot, double[]>(
            nameof(XData),
            o => o.XData,
            (o, v) => { o.XData = v; o.PlotData(); });

        public double[] YData { get; set; } = new double[0];
        public static readonly DirectProperty<ScatterPlot, double[]> YDataProperty = AvaloniaProperty.RegisterDirect<ScatterPlot, double[]>(
            nameof(YData),
            o => o.YData,
            (o, v) => { o.YData = v; o.PlotData(); });

        public ScatterPlot() : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void PlotData()
        {
            if (_scatterPlot is not null) this.Plot.Remove(_scatterPlot);
            if (XData is not null && YData is not null && XData.Length == YData.Length) _scatterPlot = this.Plot.AddScatter(XData, YData);
            this.Refresh();
        }
    }
}
