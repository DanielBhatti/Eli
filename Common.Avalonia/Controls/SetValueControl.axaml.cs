using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Common.Avalonia.Converters;
using System;

namespace Common.Avalonia.Controls;

public partial class SetValueControl : UserControl
{
    public static readonly StyledProperty<object> ValueProperty = AvaloniaProperty.Register<SetValueControl, object>(nameof(Value));

    private bool IsBinded { get; set; } = false;
    private readonly TextBox _textBox = new();
    private readonly DatePicker _datePicker = new();
    private readonly CheckBox _checkBox = new();
    private readonly ComboBox _comboBox = new();

    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public SetValueControl()
    {
        InitializeComponent();
        _ = this.GetObservable(ValueProperty).Subscribe(_ => UpdateControl());
    }

    private void InstantiateBinding()
    {
        var v = Value; // bindings seem to sometimes null out the current value, we're storing it to set it back after the binding
        if(Value is null || IsBinded) return;
        else if(Value is Enum enumValue)
        {
            _comboBox.ItemsSource = Enum.GetValues(enumValue.GetType());
            _comboBox.SelectedItem = enumValue;
            _ = _comboBox.Bind(ComboBox.SelectedItemProperty, new Binding(nameof(Value)) { Source = this });
            Content = _comboBox;
        }
        else if(Value is DateTime)
        {
            _ = _datePicker.Bind(DatePicker.SelectedDateProperty, new Binding(nameof(Value)) { Source = this });
            Content = _datePicker;
        }
        else if(Value is bool)
        {
            _ = _checkBox.Bind(CheckBox.IsCheckedProperty, new Binding(nameof(Value)) { Source = this });
            Content = _checkBox;
        }
        else if(Value is char)
        {
            _ = _textBox.Bind(TextBox.TextProperty, new Binding(nameof(Value)) { Source = this, Converter = new CharToStringConverter() });
            _textBox.Text = v?.ToString();
            Content = _textBox;
        }
        else if(Value is int or byte or short or ushort or uint or long or ulong or float or double or decimal)
        {
            _ = _textBox.Bind(TextBox.TextProperty, new Binding(nameof(Value)) { Source = this });
            _textBox.Text = v?.ToString();
            Content = _textBox;
        }
        else
        {
            _ = _textBox.Bind(TextBox.TextProperty, new Binding(nameof(Value)) { Source = this });
            Content = _textBox;
        }
        IsBinded = true;
    }

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private void UpdateControl()
    {
        if(!IsBinded) InstantiateBinding();
        if(Value is null) return;
        else if(Value is Enum) Content = _comboBox;
        else if(Value is DateTime) Content = _datePicker;
        else if(Value is bool) Content = _checkBox;
        else if(Value is char) Content = _textBox;
        else if(Value is int or byte or short or ushort or uint or long or ulong or float or double or decimal) Content = _textBox;
        else Content = _textBox;
    }
}
