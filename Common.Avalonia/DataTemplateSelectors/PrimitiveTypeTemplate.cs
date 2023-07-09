using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;

namespace Common.Avalonia.DataTemplateSelectors;

public class PrimitiveTypeTemplate : IDataTemplate
{
    public bool Match(object? data)
    {
        return data is string
            or DateTime
            or int
            or bool
            or byte
            or sbyte
            or short
            or ushort
            or uint
            or long
            or ulong
            or float
            or double
            or decimal
            or char;
    }

    Control? ITemplate<object?, Control?>.Build(object? data)
    {
        if(data is string s) return new TextBox { Text = s };
        else if(data is DateTime dt) return new DatePicker { SelectedDate = dt };
        else if(data is bool b) return new CheckBox { IsChecked = b };
        else if(data is char c) return new TextBox { Text = c.ToString() };
        else if(data is int or byte or short or ushort or ushort or uint or long or ulong or float or double or decimal) return new NumericUpDown { Value = Convert.ToDecimal(data) };
        else return new TextBox { Text = data as string };
    }
}
