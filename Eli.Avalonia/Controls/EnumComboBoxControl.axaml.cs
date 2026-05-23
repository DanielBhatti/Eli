using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Data;
using System;
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

    public Enum SelectedValue
    {
        get;
        set => _ = SetAndRaise(SelectedValueProperty, ref field, value);
    } = DummyEnum.DummyValue;

    public static readonly DirectProperty<EnumComboBoxControl, IEnumerable> ItemsProperty =
        AvaloniaProperty.RegisterDirect<EnumComboBoxControl, IEnumerable>(
            nameof(Items),
            o => o.Items,
            (o, v) => o.Items = v);

    public IEnumerable Items
    {
        get;
        set => _ = SetAndRaise(ItemsProperty, ref field, value);
    } = null!;

    public EnumComboBoxControl() => InitializeComponent();

    public enum DummyEnum
    {
        DummyValue
    }
}
