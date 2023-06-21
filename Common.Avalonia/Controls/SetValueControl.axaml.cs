using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Common.Avalonia.Converters;
using System;

namespace Common.Avalonia.Controls;

public class SetValueControl : UserControl
{
    public static readonly StyledProperty<object> ValueProperty = AvaloniaProperty.Register<SetValueControl, object>(nameof(Value));

    public SetValueControl()
    {
        InitializeComponent();
        _ = this.GetObservable(ValueProperty).Subscribe(_ => UpdateControl());
    }

    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void UpdateControl()
    {
        if(Value is null) return;
        if(Value is string)
        {
            var textBox = new TextBox();
            _ = textBox.Bind(TextBox.TextProperty, new Binding(nameof(Value)) { Source = this });
            Content = textBox;
        }
        else if(Value is DateTime)
        {
            var datePicker = new DatePicker();
            _ = datePicker.Bind(DatePicker.SelectedDateProperty, new Binding(nameof(Value)) { Source = this });
            Content = datePicker;
        }
        else if(Value is bool)
        {
            var checkBox = new CheckBox();
            _ = checkBox.Bind(CheckBox.IsCheckedProperty, new Binding(nameof(Value)) { Source = this });
            Content = checkBox;
        }
        else if(Value is char)
        {
            var textBox = new TextBox();
            _ = textBox.Bind(TextBox.TextProperty, new Binding(nameof(Value)) { Source = this, Converter = new CharToStringConverter() });
            Content = textBox;
        }
        else if(Value is int or byte or short or ushort or uint or long or ulong or float or double or decimal)
        {
            var numericUpDown = new NumericUpDown();
            _ = numericUpDown.Bind(NumericUpDown.ValueProperty, new Binding(nameof(Value)) { Source = this, Converter = new NumericToDoubleConverter() });
            Content = numericUpDown;
        }
        else if(Value is Enum enumValue)
        {
            var comboBox = new ComboBox
            {
                Items = Enum.GetValues(enumValue.GetType()),
                SelectedItem = enumValue
            };
            _ = comboBox.Bind(ComboBox.SelectedItemProperty, new Binding(nameof(Value)) { Source = this });
            Content = comboBox;
        }
        else
        {
            var textBox = new TextBox();
            _ = textBox.Bind(TextBox.TextProperty, new Binding(nameof(Value)) { Source = this });
            Content = textBox;
        }
    }
}
