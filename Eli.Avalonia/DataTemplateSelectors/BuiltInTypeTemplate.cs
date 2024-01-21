using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;

namespace Eli.Avalonia.DataTemplateSelectors;

public class BuiltInTypeTemplate : IDataTemplate
{
    public bool Match(object? data) =>
        data is bool
            or byte
            or char
            or DateTime
            or decimal
            or double
            or Enum
            or float
            or int
            or long
            or sbyte
            or short
            or string
            or uint
            or ulong
            or ushort;

    Control? ITemplate<object?, Control?>.Build(object? data)
    {
        if(data is Enum en) return new ComboBox { SelectedItem = en.ToString(), ItemsSource = Enum.GetNames(en.GetType()) };
        else if(data is DateTime dt) return new DatePicker { SelectedDate = dt };
        else if(data is bool b) return new CheckBox { IsChecked = b };
        else if(data is int or byte or short or ushort or ushort or uint or long or ulong or float or double or decimal) return new NumericUpDown { Value = Convert.ToDecimal(data) };
        else if(data is char c) return new TextBox { Text = c.ToString() };
        else return new TextBox { Text = data as string };
    }
}
