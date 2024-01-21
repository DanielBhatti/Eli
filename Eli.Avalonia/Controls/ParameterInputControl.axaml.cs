using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace Eli.Avalonia.Controls;

public partial class ParameterInputControl : UserControl
{
    public ParameterInputControl() => InitializeComponent();

    public static readonly StyledProperty<string> ParameterNameProperty = AvaloniaProperty.Register<ParameterInputControl, string>(nameof(ParameterName), defaultBindingMode: BindingMode.TwoWay);

    public string ParameterName
    {
        get => (string)GetValue(ParameterNameProperty);
        set => SetValue(ParameterNameProperty, value);
    }

    public static readonly StyledProperty<object> ValueProperty = AvaloniaProperty.Register<ParameterInputControl, object>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}
