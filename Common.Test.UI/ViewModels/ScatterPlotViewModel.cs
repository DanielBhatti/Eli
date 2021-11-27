using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Test.UI.ViewModels
{
    public class ScatterPlotViewModel : ViewModelBase
    {
        public double[] XData { get; set; } = new double[3] { 1, 2, 3 };
        public double[] YData { get; set; } = new double[3] { 1, 2, 3 };

        public string Title { get; set; } = "Title";
        public string XAxis { get; set; } = "XAxis";
        public string YAxis { get; set; } = "YAxis";
    }
}
