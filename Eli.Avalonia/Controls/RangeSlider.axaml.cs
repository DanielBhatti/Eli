using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using System;

namespace Eli.Avalonia.Controls;

public partial class RangeSlider : UserControl
{
    public RangeSlider() => InitializeComponent();

    public static readonly StyledProperty<int> MinimumProperty = AvaloniaProperty.Register<ParameterInputControl, int>(nameof(Minimum), defaultBindingMode: BindingMode.TwoWay);

    public int Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly StyledProperty<int> MaximumProperty = AvaloniaProperty.Register<ParameterInputControl, int>(nameof(Maximum), defaultBindingMode: BindingMode.TwoWay);

    public int Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    private void LowerSlider_ValueChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        var newValue = e.NewValue as int?;
        if(newValue.HasValue && newValue > UpperSlider.Value) LowerSlider.Value = Math.Max(Minimum, UpperSlider.Value - 1);
    }

    private void UpperSlider_ValueChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        var newValue = e.NewValue as int?;
        if(newValue.HasValue && newValue < LowerSlider.Value) UpperSlider.Value = Math.Min(Maximum, LowerSlider.Value + 1);
    }
}
