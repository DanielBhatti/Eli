using System.Windows;
using System.Windows.Input;

namespace Common.Wpf
{
    public class MultiSelectControlItem : DependencyObject
    {
        public string Text { get; set; }
        public bool IsChecked { get; set; }
        public ICommand CheckChangedCommand { get; set; }

        public MultiSelectControlItem(string text, bool isChecked = false, ICommand checkChangedCommand = null)
        {
            Text = text;
            IsChecked = isChecked;
            CheckChangedCommand = checkChangedCommand;
        }
    }
}
