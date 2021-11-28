using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Test.UI.ViewModels
{
    public class MultiScatterPlotViewModel : ViewModelBase
    {
        public ObservableCollection<double[]> XData { get; set; } = new();
        public ObservableCollection<double[]> YData { get; set; } = new();

        public string Title { get; set; } = "Title";
        public string XAxis { get; set; } = "XAxis";
        public string YAxis { get; set; } = "YAxis";

        public bool RefreshDataToggle { get; set; }

        public MultiScatterPlotViewModel()
        {
            XData.Add(new double[3] { 1, 2, 3 });
            XData.Add(new double[3] { 1, 2, 3 });
            XData.Add(new double[3] { 1, 2, 3 });
            XData.Add(new double[3] { 4, 5, 6 });

            YData.Add(new double[3] { 1, 2, 3 });
            YData.Add(new double[3] { 4, 5, 6 });
            YData.Add(new double[3] { 7, 8, 9 });
            YData.Add(new double[3] { 11, 12, 13 });

            RefreshDataToggle = true;
        }
    }
}
