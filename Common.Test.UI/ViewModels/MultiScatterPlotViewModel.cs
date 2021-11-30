using Common.Avalonia.Plot;
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
        public ObservableCollection<ScatterData> ScatterDataCollection { get; set; } = new();

        public string Title { get; set; } = "Title";
        public string XAxis { get; set; } = "XAxis";
        public string YAxis { get; set; } = "YAxis";
        public bool IsXDateTime { get; set; } = false;

        public bool RefreshDataToggle { get; set; }

        public MultiScatterPlotViewModel()
        {
            //ScatterData.Add(new ScatterData(new double[] { 1, 2, 3 }, new double[] { 1, 2, 3 }));
            //ScatterData.Add(new ScatterData(new double[] { 1, 2, 3 }, new double[] { 4, 5, 6 }));
            //ScatterData.Add(new ScatterData(new double[] { 4, 5, 6 }, new double[] { 4, 5, 6 }));

            double[] xData = Enumerable.Range(0, 100).Select(i => new DateTime(1900, 1, 1).AddDays(i)).Select(d => d.ToOADate()).ToArray();
            double[] yData = Enumerable.Range(0, 100).Select(i => (double)i).ToArray();
            ScatterData scatterData = new ScatterData(xData, yData);
            ScatterDataCollection.Add(scatterData);
            IsXDateTime = true;

            RefreshDataToggle = true;
        }
    }
}
