using Avalonia.Controls;
using System;
using Avalonia.Data;
using Avalonia;
using Avalonia.Collections;
using System.Collections;
using System.Linq;

namespace Eli.Avalonia.Controls;

public partial class EnumComboBoxControl : UserControl
{
    public static readonly DirectProperty<EnumComboBoxControl, Enum> SelectedValueProperty =
        AvaloniaProperty.RegisterDirect<EnumComboBoxControl, Enum>(
            nameof(SelectedValue),
            o => o.SelectedValue,
            (o, v) =>
            {
                if(o is not null && v is not null && o.GetType() != v.GetType() && o.Items == null) o.Items = new AvaloniaList<object>(Enum.GetValues(v.GetType()).Cast<Enum>().ToArray());
                if(o is not null && v is not null) o.SelectedValue = v;
            },
            defaultBindingMode: BindingMode.TwoWay);
    private Enum _selectedValue = DummyEnum.DummyValue;
    public Enum SelectedValue
    {
        get => _selectedValue;
        set => _ = SetAndRaise(SelectedValueProperty, ref _selectedValue, value);
    }

    public static readonly DirectProperty<EnumComboBoxControl, IEnumerable> ItemsProperty =
        AvaloniaProperty.RegisterDirect<EnumComboBoxControl, IEnumerable>(
            nameof(Items),
            o => o.Items,
            (o, v) => o.Items = v);
    private IEnumerable _items = null!;
    public IEnumerable Items
    {
        get => _items;
        set => _ = SetAndRaise(ItemsProperty, ref _items, value);
    }

    public EnumComboBoxControl() => InitializeComponent();

    public enum DummyEnum
    {
        DummyValue
    }
}
