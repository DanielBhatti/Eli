using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;
using Common.Avalonia.Converters;
using Avalonia;

namespace Common.Avalonia.Controls
{
    public class SetValueControl : UserControl
    {
        public static readonly StyledProperty<object> ValueProperty = AvaloniaProperty.Register<SetValueControl, object>(nameof(Value));

        public SetValueControl()
        {
            InitializeComponent();
            this.GetObservable(ValueProperty).Subscribe(_ => UpdateControl());
        }

        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void UpdateControl()
        {
            if(Value is null) return;

            if(Value is string s)
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding(nameof(Value)) { Source = this });
                this.Content = textBox;
            }
            else if(Value is DateTime dt)
            {
                var datePicker = new DatePicker();
                datePicker.Bind(DatePicker.SelectedDateProperty, new Binding(nameof(Value)) { Source = this });
                this.Content = datePicker;
            }
            else if(Value is bool b)
            {
                var checkBox = new CheckBox();
                checkBox.Bind(CheckBox.IsCheckedProperty, new Binding(nameof(Value)) { Source = this });
                this.Content = checkBox;
            }
            else if(Value is char c)
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding(nameof(Value)) { Source = this, Converter = new CharToStringConverter() });
                this.Content = textBox;
            }
            else if(Value is int || Value is byte || Value is short || Value is ushort || Value is uint || Value is long || Value is ulong || Value is float || Value is double || Value is decimal)
            {
                var numericUpDown = new NumericUpDown();
                numericUpDown.Bind(NumericUpDown.ValueProperty, new Binding(nameof(Value)) { Source = this, Converter = new NumericToDoubleConverter() });
                this.Content = numericUpDown;
            }
            else if(Value is Enum enumValue)
            {
                var comboBox = new ComboBox();
                comboBox.Items = Enum.GetValues(enumValue.GetType());
                comboBox.SelectedItem = enumValue;
                comboBox.Bind(ComboBox.SelectedItemProperty, new Binding(nameof(Value)) { Source = this });
                this.Content = comboBox;
            }
            else
            {
                var textBox = new TextBox();
                textBox.Bind(TextBox.TextProperty, new Binding(nameof(Value)) { Source = this });
                this.Content = textBox;
            }
        }
    }
}
