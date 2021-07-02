using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Wpf
{
    public class PlotData<T1, T2>
    {
        public IList<T1> XData { get; set; }
        public IList<T2> YData { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public LineStyle LineStyle { get; set; }
        public double MarkerSize { get; set; }
        public string XAxisKey { get; set; }
        public string YAxisKey { get; set; }

        public PlotData(IList<T1> xData, IList<T2> yData, string name,
            Color color = Color.Black, LineStyle lineStyle = LineStyle.Solid,
            double markerSize = 2, string xAxisKey = null, string yAxisKey = null)
        {
            XData = xData;
            YData = yData;
            Name = name;
            Color = color;
            LineStyle = lineStyle;
            MarkerSize = markerSize;
            XAxisKey = xAxisKey;
            YAxisKey = yAxisKey;
        }
    }
}
