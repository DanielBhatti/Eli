using ReactiveUI;

namespace Common.Test.UI.ViewModels
{
    public class ScatterPlotViewModel : ViewModelBase
    {
        private double[] _xData = new double[3] { 1, 2, 3 };
        public double[] XData
        {
            get => _xData;
            set => this.RaiseAndSetIfChanged(ref _xData, value);
        }
        private double[] _yData = new double[3] { 1, 2, 3 };
        public double[] YData
        {
            get => _yData;
            set => this.RaiseAndSetIfChanged(ref _yData, value);
        }

        public string Title { get; set; } = "Title";
        public string XAxis { get; set; } = "XAxis";
        public string YAxis { get; set; } = "YAxis";
    }
}
