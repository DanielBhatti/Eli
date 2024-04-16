using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace Eli.Avalonia.Controls;

public partial class SetValueControl : UserControl
{
    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public static readonly StyledProperty<object> ValueProperty = AvaloniaProperty.Register<SetValueControl, object>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);
    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<string> WatermarkProperty = AvaloniaProperty.Register<SetValueControl, string>(nameof(Watermark), defaultBindingMode: BindingMode.TwoWay);
    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public static readonly StyledProperty<bool> UseReflectionProperty = AvaloniaProperty.Register<SetValueControl, bool>(nameof(UseReflection), defaultBindingMode: BindingMode.TwoWay);
    public bool UseReflection
    {
        get => GetValue(UseReflectionProperty);
        set => SetValue(UseReflectionProperty, value);
    }

    public SetValueControl() => InitializeComponent();
}
