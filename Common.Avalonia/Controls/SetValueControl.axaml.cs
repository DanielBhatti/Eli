using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;

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

    public static readonly StyledProperty<bool> UseReflectionProperty = AvaloniaProperty.Register<SetValueControl, bool>(nameof(UseReflection), defaultBindingMode: BindingMode.TwoWay);

    public bool UseReflection
    {
        get => GetValue(UseReflectionProperty);
        set => SetValue(UseReflectionProperty, value);
    }

    public SetValueControl() => InitializeComponent();
}
