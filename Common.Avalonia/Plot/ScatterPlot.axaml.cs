using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;

namespace Common.Avalonia.Plot
{
    public partial class ScatterPlot : UserControl
    {
        private AvaPlot _avaPlot;
        private ScottPlot.Plottable.ScatterPlot? _scatterPlot;

        public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<ScatterPlot, string>(nameof(Title));
        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly StyledProperty<string> XAxisProperty = AvaloniaProperty.Register<ScatterPlot, string>(nameof(XAxis));
        public string XAxis
        {
            get => GetValue(XAxisProperty);
            set => SetValue(XAxisProperty, value);
        }

        public static readonly StyledProperty<string> YAxisProperty = AvaloniaProperty.Register<ScatterPlot, string>(nameof(YAxis));
        public string YAxis
        {
            get => GetValue(YAxisProperty);
            set => SetValue(YAxisProperty, value);
        }

        public static readonly StyledProperty<double[]> XDataProperty = AvaloniaProperty.Register<ScatterPlot, double[]>(nameof(XData));
        public double[] XData
        {
            get => GetValue(XDataProperty);
            set
            {
                SetValue(XDataProperty, value);
                PlotData();
            }
        }

        public static readonly StyledProperty<double[]> YDataProperty = AvaloniaProperty.Register<ScatterPlot, double[]>(nameof(YData));
        public double[] YData
        {
            get => GetValue(YDataProperty);
            set
            {
                SetValue(YDataProperty, value);
                PlotData();
            }
        }

        public static readonly StyledProperty<string> ErrorTextProperty = AvaloniaProperty.Register<ScatterPlot, string>(nameof(ErrorText));
        public string ErrorText
        {
            get => GetValue(ErrorTextProperty);
            private set => SetValue(ErrorTextProperty, value);
        }

        public ScatterPlot()
        {
            InitializeComponent();
            _avaPlot = this.Find<AvaPlot>("AvaScatterPlot");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void PlotData()
        {
            if (_scatterPlot is not null) _avaPlot.Plot.Remove(_scatterPlot);
            _scatterPlot = _avaPlot.Plot.AddScatter(XData, YData);
            _avaPlot.Refresh();
        }
        
        private void AddAxis(AxisType axisType)
        {
            ErrorText = "";
            switch(axisType)
            {
                case AxisType.XPrimary:
                    _avaPlot.Plot.AddAxis(ScottPlot.Renderable.Edge.Left, 0, XAxis);
                    break;
                case AxisType.YPrimary:
                    _avaPlot.Plot.AddAxis(ScottPlot.Renderable.Edge.Bottom, 0, YAxis);
                    break;
                case AxisType.Title:
                    _avaPlot.Plot.Title(Title, true);
                    break;
                default:
                    ErrorText = "Failed to add axis.  Support axis types are XPrimary, YPrimary, and Title.";
                    break;
            }
        }
    }
}
