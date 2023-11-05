using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;

namespace Common.Avalonia.Controls;

public partial class SetValueControl : UserControl
{
    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    public static readonly StyledProperty<object> ValueProperty = AvaloniaProperty.Register<SetValueControl, object>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public SetValueControl() => InitializeComponent();
}
