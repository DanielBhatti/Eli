using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace Eli.Avalonia.Controls;

public partial class IntegerNumericUpDown : NumericUpDown
{
    public static new readonly StyledProperty<int> ValueProperty =
        AvaloniaProperty.Register<IntegerNumericUpDown, int>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public new int Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    static IntegerNumericUpDown() => ValueProperty.Changed.AddClassHandler<IntegerNumericUpDown>((x, e) => x.OnIntValueChanged(e));

    public IntegerNumericUpDown() => base.ValueChanged += OnBaseValueChanged;

    private void OnBaseValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if(e.NewValue.HasValue)
        {
            Value = (int)e.NewValue.Value;
        }
    }

    private void OnIntValueChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if(e.NewValue is int newValue)
        {
            base.Value = newValue;
        }
    }
}
