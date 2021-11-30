using Avalonia;
using Avalonia.Controls;

namespace Common.Avalonia.Plot
{
    public class ScatterData : AvaloniaObject
    {
        public int XAxisIndex { get; set; } = 0;
        public static readonly DirectProperty<ScatterData, int> XAxisIndexProperty = AvaloniaProperty.RegisterDirect<ScatterData, int>(
            nameof(XData),
            o => o.XAxisIndex,
            (o, v) => o.XAxisIndex = v);

        public int YAxisIndex { get; set; } = 0;
        public static readonly DirectProperty<ScatterData, int> YAxisIndexProperty = AvaloniaProperty.RegisterDirect<ScatterData, int>(
            nameof(YData),
            o => o.YAxisIndex,
            (o, v) => o.YAxisIndex = v);

        public double[] XData { get; set; } = new double[0];
        public static readonly DirectProperty<ScatterData, double[]> XDataProperty = AvaloniaProperty.RegisterDirect<ScatterData, double[]>(
            nameof(XData),
            o => o.XData,
            (o, v) => o.XData = v);

        public double[] YData { get; set; } = new double[0];
        public static readonly DirectProperty<ScatterData, double[]> YDataProperty = AvaloniaProperty.RegisterDirect<ScatterData, double[]>(
            nameof(YData),
            o => o.YData,
            (o, v) => o.YData = v);

        public ScatterData(double[] xData, double[] yData, int xAxisIndex = 0, int yAxisIndex = 0)
        {
            XData = xData;
            YData = yData;
            XAxisIndex = xAxisIndex;
            YAxisIndex = yAxisIndex;
        }
    }
}
