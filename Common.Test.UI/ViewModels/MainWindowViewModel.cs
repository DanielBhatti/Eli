using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Test.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ScatterPlotViewModel ScatterPlotViewModel { get; } = new ScatterPlotViewModel();

        public void TestLargeDataSet(int n)
        {
            Random random = new Random();
            ScatterPlotViewModel.XData = Enumerable.Range(0, n).Select(i => (double)i).ToArray();
            ScatterPlotViewModel.YData = Enumerable.Repeat(0, n).Select(i => 100 * random.NextDouble()).ToArray();
        }

        public MainWindowViewModel()
        {
            //TestLargeDataSet(100);
        }
    }
}
