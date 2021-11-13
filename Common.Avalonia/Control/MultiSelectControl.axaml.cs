using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Common.Avalonia.Control
{
    public partial class MultiSelectControl : UserControl
    {
        public MultiSelectControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
