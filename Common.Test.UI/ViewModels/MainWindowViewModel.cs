using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Test.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ScatterPlotViewModel ScatterPlotViewModel { get; } = new ScatterPlotViewModel();
    }
}
