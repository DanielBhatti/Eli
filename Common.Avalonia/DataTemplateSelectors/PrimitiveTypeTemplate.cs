using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;

namespace Common.Avalonia.DataTemplateSelectors
{
    public class PrimitiveTypeTemplate : IDataTemplate
    {
        public bool Match(object data)
        {
            return data is string
                || data is DateTime
                || data is int
                || data is bool
                || data is byte
                || data is sbyte
                || data is short
                || data is ushort
                || data is uint
                || data is long
                || data is ulong
                || data is float
                || data is double
                || data is decimal
                || data is char;
        }

        public IControl Build(object data)
        {
            if(data is string s) return new TextBox { Text = s };
            else if(data is DateTime dt) return new DatePicker { SelectedDate = dt };
            else if(data is bool b) return new CheckBox { IsChecked = b };
            else if(data is char c) return new TextBox { Text = c.ToString() };
            else if(data is int || data is byte || data is short || data is ushort || data is ushort || data is uint || data is long || data is ulong || data is float || data is double || data is decimal) return new NumericUpDown { Value = Convert.ToDouble(data) };
            else return new TextBox { Text = data as string };
        }
    }
}
