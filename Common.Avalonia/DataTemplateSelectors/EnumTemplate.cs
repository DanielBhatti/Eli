using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.Linq;

namespace Common.Avalonia.DataTemplateSelectors;

internal class EnumTemplate : IDataTemplate
{
    public Control? Build(object? param)
    {
        var e = param as Enum;
        var comboBox = new ComboBox { SelectedValue = e, ItemsSource = Enum.GetValues(e.GetType()).Cast<Enum>().ToList() };
        return comboBox;
    }

    public bool Match(object? data) => data is Enum;
}
