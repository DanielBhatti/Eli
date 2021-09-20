using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Common.Avalonia.Text;
using System;

namespace Common.Avalonia.Control
{
    public partial class CellControl : UserControl
    {
        public static StyledProperty<string> TextProperty = AvaloniaProperty.Register<CellControl, string>(nameof(Text));
        public string Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static StyledProperty<ValueString> ValueStringProperty = AvaloniaProperty.Register<CellControl, ValueString>(nameof(ValueString));
        public ValueString ValueString
        {
            get => GetValue(ValueStringProperty);
            set => SetValue(ValueStringProperty, value);
        }

        public CellControl()
        {
            InitializeComponent();
            ValueString = new ValueString("", typeof(string));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
