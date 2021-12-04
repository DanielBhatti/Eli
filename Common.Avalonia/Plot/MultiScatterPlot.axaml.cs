using Avalonia;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Common.Avalonia.Plot
{
    public partial class MultiScatterPlot : AvaPlot
    {
        public string Title { get; set; } = "";
        public static readonly DirectProperty<MultiScatterPlot, string> TitleProperty = AvaloniaProperty.RegisterDirect<MultiScatterPlot, string>(
            nameof(Title),
            o => o.Title,
            (o, v) => { o.Title = v; o.Plot.Title(v, true); });

        public string BottomAxis { get; set; } = "";
        public static readonly DirectProperty<MultiScatterPlot, string> BottomAxisProperty = AvaloniaProperty.RegisterDirect<MultiScatterPlot, string>(
            nameof(BottomAxis),
            o => o.BottomAxis,
            (o, v) => { o.BottomAxis = v; o.Plot.XAxis.Label(v); });

        public string LeftAxis { get; set; } = "";
        public static readonly DirectProperty<MultiScatterPlot, string> LeftAxisProperty = AvaloniaProperty.RegisterDirect<MultiScatterPlot, string>(
            nameof(LeftAxis),
            o => o.LeftAxis,
            (o, v) => { o.LeftAxis = v; o.Plot.YAxis.Label(v); });

        public string RightAxis { get; set; } = "";
        public static readonly DirectProperty<MultiScatterPlot, string> RightAxisProperty = AvaloniaProperty.RegisterDirect<MultiScatterPlot, string>(
            nameof(RightAxis),
            o => o.RightAxis,
            (o, v) => { o.RightAxis = v; o.Plot.YAxis.Label(v); });

        public ICollection<ScatterData> ScatterDataCollection { get; set; } = new List<ScatterData>();
        public static readonly DirectProperty<MultiScatterPlot, ICollection<ScatterData>> ScatterDataCollectionProperty = AvaloniaProperty.RegisterDirect<MultiScatterPlot, ICollection<ScatterData>>(
            nameof(ScatterDataCollection),
            o => o.ScatterDataCollection,
            (o, v) => o.ScatterDataCollection = v);

        private bool _isXDateTime = false;
        public bool IsXDateTime
        {
            get => _isXDateTime;
            set { this.Plot.XAxis.DateTimeFormat(value); _isXDateTime = value; }
        }
        public static readonly DirectProperty<MultiScatterPlot, bool> IsXDateTimeProperty = AvaloniaProperty.RegisterDirect<MultiScatterPlot, bool>(
            nameof(IsXDateTime),
            o => o.IsXDateTime,
            (o, v) => o.IsXDateTime = v);

        public bool RefreshDataToggle { get; set; } = false;
        public static readonly DirectProperty<MultiScatterPlot, bool> RefreshDataToggleProperty = AvaloniaProperty.RegisterDirect<MultiScatterPlot, bool>(
            nameof(RefreshDataToggle),
            o => o.RefreshDataToggle,
            (o, v) => { if (v == true) o.RefreshData(); o.RefreshDataToggle = false; });

        public string ErrorText { get; private set; } = "";
        public static readonly DirectProperty<MultiScatterPlot, string> ErrorTextProperty = AvaloniaProperty.RegisterDirect<MultiScatterPlot, string>(
            nameof(ErrorText),
            o => o.ErrorText);

        private Color[] Colors = new Color[]
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

        public MultiScatterPlot()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void RefreshData()
        {
            this.Plot.Clear();
            ErrorText = "";
            for (int i = 0; i < ScatterDataCollection.Count; i++)
            {
                ScatterData scatterData = ScatterDataCollection.ElementAt(i);
                try
                {
                    this.Plot.AddScatter(scatterData.XData, scatterData.YData, Colors[i % Colors.Length]);
                }
                catch
                {
                    ErrorText = $"Failed to plot item {i}.\n";
                }
            }
        }
    }
}
